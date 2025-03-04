﻿using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Update;

public class UpdateRecipe(ILoggedUser _loggedUser, IUnitOfWork unitOfWork, IMapper mapper, IRecipeUpdateOnlyRepository recipeUpdateOnlyRepository) : IUpdateRecipe
{
  public async Task Execute(long id, RecipeRequest request)
  {
    Validate(request);

    var loggedUser = await _loggedUser.User();
    var recipe = await recipeUpdateOnlyRepository.GetById(loggedUser, id);

    if (recipe == null)
    {
      throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    recipe.Ingredients.Clear();
    recipe.Instructions.Clear();
    recipe.DishTypes.Clear();

    mapper.Map(request, recipe);

    var instructions = request.Instructions.OrderBy(i => i.Step).ToList();

    for (var i = 0; i < instructions.Count; i++)
    {
      instructions[i].Step = i + 1;
    }

    recipe.Instructions = mapper.Map<IList<Instruction>>(instructions);

    recipeUpdateOnlyRepository.Update(recipe);

    await unitOfWork.CommitAsync();
  }

  private static void Validate(RecipeRequest request)
  {
    var result = new RecipeValidator().Validate(request);

    if (result.IsValid.IsFalse())
    {
      throw new RequestValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
  }
}
