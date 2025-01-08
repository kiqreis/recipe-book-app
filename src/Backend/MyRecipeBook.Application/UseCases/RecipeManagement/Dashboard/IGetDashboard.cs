using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Dashboard;

public interface IGetDashboard
{
  Task<RecipesResponse> Execute();
}
