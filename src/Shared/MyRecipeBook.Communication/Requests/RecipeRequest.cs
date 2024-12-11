﻿using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Requests;

public class RecipeRequest
{
  public string Title { get; set; } = string.Empty;
  public CookingTime? CookingTime { get; set; }
  public Difficulty? Difficulty { get; set; }
  public IList<string> Ingredients { get; set; } = [];
  public IList<InstructionRequest> Instructions { get; set; } = [];
  public IList<DishType> DishTypes { get; set; } = [];
}
