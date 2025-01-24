using AutoMapper;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Filter;

public class FilterRecipe(IMapper mapper, ILoggedUser _loggedUser, IRecipeReadOnlyRepository recipeReadOnlyRepository, IBlobStorageService blobStorageService) : IFilterRecipe
{
  public async Task<RecipesResponse> Execute(RecipeFilterRequest request)
  {
    Validate(request);

    var loggedUser = await _loggedUser.User();

    var filters = new FilterRecipeDto
    {
      RecipeTitleIngredient = request.RecipeTitleIngredient,
      CookingTimes = request.CookingTimes.Distinct().Select(c => (CookingTime)c).ToList(),
      Difficulties = request.Difficulties.Distinct().Select(d => (Difficulty)d).ToList(),
      DishTypes = request.DishTypes.Distinct().Select(d => (DishType)d).ToList()
    };

    var recipes = await recipeReadOnlyRepository.Filter(loggedUser, filters);

    return new RecipesResponse
    {
      Recipes = await recipes.MapToShortRecipe(loggedUser, blobStorageService, mapper)
    };
  }

  private static void Validate(RecipeFilterRequest request)
  {
    var validator = new FilterRecipeValidator();
    var result = validator.Validate(request);

    if (result.IsValid.IsFalse())
    {
      var errorMessages = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();

      throw new RequestValidationException(errorMessages);
    }
  }
}
