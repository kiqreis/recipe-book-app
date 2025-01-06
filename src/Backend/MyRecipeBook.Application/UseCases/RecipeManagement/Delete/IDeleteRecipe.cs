namespace MyRecipeBook.Application.UseCases.RecipeManagement.Delete;

public interface IDeleteRecipe
{
  Task Execute(long recipeId);
}