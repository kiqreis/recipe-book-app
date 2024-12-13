using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.RecipeRepository;

public interface IRecipeRepository
{
  Task Add(Recipe recipe);
}
