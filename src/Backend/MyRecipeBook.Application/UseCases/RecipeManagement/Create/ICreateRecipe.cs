using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Create;

public interface ICreateRecipe
{
  public Task<CreatedRecipeResponse> Execute(RecipeRequest request);
}
