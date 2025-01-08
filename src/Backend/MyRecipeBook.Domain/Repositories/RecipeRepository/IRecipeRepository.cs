using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.RecipeRepository;

public interface IRecipeRepository
{
  Task Add(Recipe recipe);
  Task<IList<Recipe>> Filter(User user, FilterRecipeDto filters);
  Task<Recipe?> GetById(User user, long recipeId);
  Task<Recipe?> GetByIdUpdate(User user, long recipeId);
  Task Delete(long recipeId);
  Task<IList<Recipe>> GetForDashboard(User user);
  void Update(Recipe recipe);
}
