using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Dashboard;

public class GetDashboard(IRecipeRepository repository, IMapper mapper, ILoggedUser _loggedUser) : IGetDashboard
{
  public async Task<RecipesResponse> Execute()
  {
    var loggedUser = await _loggedUser.User();
    var recipes = await repository.GetForDashboard(loggedUser);

    return new RecipesResponse
    {
      Recipes = mapper.Map<IList<RecipeResponseShort>>(recipes)
    };
  }
}
