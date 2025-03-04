﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.RecipeRepository;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(AppDbContext context) : IRecipeWriteOnlyRepository, IRecipeUpdateOnlyRepository, IRecipeReadOnlyRepository
{
  public async Task Add(Recipe recipe) => await context.Recipes.AddAsync(recipe);

  public async Task Delete(long recipeId)
  {
    var recipe = await context.Recipes.FindAsync(recipeId);

    context.Recipes.Remove(recipe!);
  }

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

    if (filters.RecipeTitleIngredient.NotEmpty())
    {
      query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitleIngredient) || recipe.Ingredients.Any(ingredient =>
        ingredient.Item.Contains(filters.RecipeTitleIngredient)));
    }

    return await query.ToListAsync();
  }

  async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
  {
    return await GetFullRecipe()
        .AsNoTracking()
        .FirstOrDefaultAsync(recipe => recipe.IsActive && recipe.Id == recipeId && recipe.UserId == user.Id);
  }

  async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
  {
    return await GetFullRecipe()
        .FirstOrDefaultAsync(recipe => recipe.IsActive && recipe.Id == recipeId && recipe.UserId == user.Id);
  }

  public void Update(Recipe recipe) => context.Recipes.Update(recipe);

  public async Task<IList<Recipe>> GetForDashboard(User user)
  {
    return await context.Recipes
      .AsNoTracking()
      .Include(recipe => recipe.Ingredients)
      .Where(recipe => recipe.IsActive && recipe.UserId == user.Id)
      .OrderByDescending(recipe => recipe.CreatedAt)
      .Take(5)
      .ToListAsync();
  }

  private IIncludableQueryable<Recipe, IList<DishType>> GetFullRecipe()
  {
    return context.Recipes
      .Include(recipe => recipe.Ingredients)
      .Include(recipe => recipe.Instructions)
      .Include(recipe => recipe.DishTypes);
  }
}
