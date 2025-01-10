using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Create;

public class CreateRecipe(IRecipeRepository repository, ILoggedUser _loggedUser, IUnitOfWork unitOfWork, IMapper mapper) : ICreateRecipe
{
  public async Task<CreatedRecipeResponse> Execute(RecipeRequest request)
  {
    Validate(request);

    var loggedUser = await _loggedUser.User();
    var recipe = mapper.Map<Recipe>(request);

    recipe.UserId = loggedUser.Id;

    var instructions = request.Instructions.OrderBy(instruction => instruction.Step).ToList();

    for (var i = 0; i < instructions.Count; i++)
    {
      instructions[i].Step = i + 1;
    }

    recipe.Instructions = mapper.Map<IList<Instruction>>(instructions);

    await repository.Add(recipe);
    await unitOfWork.CommitAsync();

    return mapper.Map<CreatedRecipeResponse>(recipe);
  }

  public static void Validate(RecipeRequest request)
  {
    var result = new RecipeValidator().Validate(request);

    if (result.IsValid == false)
    {
      throw new RequestValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
  }
}
