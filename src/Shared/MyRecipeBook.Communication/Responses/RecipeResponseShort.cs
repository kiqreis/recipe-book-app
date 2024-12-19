namespace MyRecipeBook.Communication.Responses;

public class RecipeResponseShort
{
  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public int AmountIngredients { get; set; }
}
