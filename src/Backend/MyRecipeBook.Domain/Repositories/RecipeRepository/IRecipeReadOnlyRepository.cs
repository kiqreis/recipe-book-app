using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.RecipeRepository;

public interface IRecipeReadOnlyRepository
{
  Task<IList<Recipe>> Filter(User user, FilterRecipeDto filter);
  Task<Recipe?> GetById(User user, long recipeId);
  Task<IList<Recipe>> GetForDashboard(User user);
}
