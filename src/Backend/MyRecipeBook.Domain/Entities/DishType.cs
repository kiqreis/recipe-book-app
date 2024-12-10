namespace MyRecipeBook.Domain.Entities;

public class DishType : EntityBase
{
  public DishType Type { get; set; } = default!;
  public long RecipeId { get; set; }
}
