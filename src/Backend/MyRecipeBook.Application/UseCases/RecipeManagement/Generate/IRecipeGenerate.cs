using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Generate;

public interface IRecipeGenerate
{
  Task<RecipeGeneratedResponse> Execute(RecipeGeneratedRequest request);
}
