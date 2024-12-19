using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Filter;

public interface IFilterRecipe
{
  Task<RecipesResponse> Execute(RecipeFilterRequest request);
}
