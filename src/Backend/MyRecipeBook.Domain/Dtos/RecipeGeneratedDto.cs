using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos;

public class RecipeGeneratedDto
{
  public string Title { get; set; } = string.Empty;
  public IList<string> Ingredients { get; set; } = [];
  public IList<InstructionGeneratedDto> Instructions { get; set; } = [];
  public CookingTime CookingTime { get; set; }
}
