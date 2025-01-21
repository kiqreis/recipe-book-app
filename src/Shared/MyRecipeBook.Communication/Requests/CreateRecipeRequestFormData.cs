using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Communication.Requests;

public class CreateRecipeRequestFormData : RecipeRequest
{
  public IFormFile? Image { get; set; }
}
