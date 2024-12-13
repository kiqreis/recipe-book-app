using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities;

[Table("DishTypes")]
public class DishType : EntityBase
{
  public DishType Type { get; set; } = default!;
  public long RecipeId { get; set; }
}
