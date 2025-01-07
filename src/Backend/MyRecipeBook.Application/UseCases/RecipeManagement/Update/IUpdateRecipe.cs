using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Update;

public interface IUpdateRecipe
{
  Task Execute(long id, RecipeRequest request);
}
