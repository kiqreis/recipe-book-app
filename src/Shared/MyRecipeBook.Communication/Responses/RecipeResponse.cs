﻿using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Responses;

public class RecipeResponse
{
  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public IList<IngredientResponse> Ingredients { get; set; } = [];
  public IList<InstructionResponse> Instructions { get; set; } = [];
  public IList<DishType> DishTypes { get; set; } = [];
  public CookingTime? CookingTime { get; set; }
  public Difficulty? Difficulty { get; set; }
}
