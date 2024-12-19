using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Filter;

public class FilterRecipe(IMapper mapper, ILoggedUser _loggedUser) : IFilterRecipe
{
  public async Task<RecipesResponse> Execute(RecipeFilterRequest request)
  {
    Validate(request);

    var loggedUser = await _loggedUser.User();

    return new RecipesResponse
    {
      Recipes = []
    };
  }

  private static void Validate(RecipeFilterRequest request)
  {
    var validator = new FilterRecipeValidator();
    var result = validator.Validate(request);

    if (result.IsValid == false)
    {
      var errorMessages = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();

      throw new RequestValidationException(errorMessages);
    }
  }
}
