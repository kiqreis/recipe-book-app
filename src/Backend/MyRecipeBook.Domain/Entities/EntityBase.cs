namespace MyRecipeBook.Domain.Entities;

public class EntityBase
{
  public long Id { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}