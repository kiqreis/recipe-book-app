using AutoMapper;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Dashboard;

public class GetDashboard(IRecipeReadOnlyRepository recipeReadOnlyRepository, IMapper mapper, ILoggedUser _loggedUser, IBlobStorageService blobStorageService) : IGetDashboard
{
  public async Task<RecipesResponse> Execute()
  {
    var loggedUser = await _loggedUser.User();
    var recipes = await recipeReadOnlyRepository.GetForDashboard(loggedUser);

    return new RecipesResponse
    {
      Recipes = await recipes.MapToShortRecipe(loggedUser, blobStorageService, mapper)
    };
  }
}
