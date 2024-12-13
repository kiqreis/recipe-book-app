using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.RecipeRepository;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(AppDbContext context) : IRecipeRepository
{
  public async Task Add(Recipe recipe) => await context.Recipes.AddAsync(recipe);
}
