using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.GetById;

public interface IGetRecipeById
{
  Task<RecipeResponse> Execute(long id);
}
