namespace MyRecipeBook.Communication.Responses;

public class RecipesResponse
{
  public IList<RecipeResponseShort> Recipes { get; set; } = [];
}
