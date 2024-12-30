﻿using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.RecipeRepository;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(AppDbContext context) : IRecipeRepository
{
  public async Task Add(Recipe recipe) => await context.Recipes.AddAsync(recipe);

  public async Task<IList<Recipe>> Filter(User user, FilterRecipeDto filters)
  {
    var query = context.Recipes.AsNoTracking()
      .Include(recipe => recipe.Ingredients)
      .Where(recipe => recipe.IsActive && recipe.UserId == user.Id);

    if (filters.Difficulties.Any())
    {
      query = query.Where(recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));
    }

    if (filters.CookingTimes.Any())
    {
      query = query.Where(recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));
    }

    if (filters.DishTypes.Any())
    {
      query = query.Where(recipe => recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));
    }

    if (!string.IsNullOrWhiteSpace(filters.RecipeTitleIngredient))
    {
      query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitleIngredient) || recipe.Ingredients.Any(ingredient =>
        ingredient.Item.Contains(filters.RecipeTitleIngredient)));
    }

    return await query.ToListAsync();
  }

  public async Task<Recipe?> GetById(User user, long recipeId)
  {
    return await context.Recipes.AsNoTracking()
      .Include(recipe => recipe.Ingredients)
      .Include(recipe => recipe.Instructions)
      .Include(recipe => recipe.DishTypes)
      .FirstOrDefaultAsync(recipe => recipe.IsActive && recipe.Id == recipeId && recipe.UserId == user.Id);
  }
}