using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos;

public record FilterRecipeDto
{
  public string? RecipeTitleIngredient { get; set; }
  public IList<CookingTime> CookingTimes { get; set; } = [];
  public IList<Difficulty> Difficulties { get; set; } = [];
  public IList<DishType> DishTypes { get; set; } = [];
}
