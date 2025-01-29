using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public class CreateRecipeFormDataRequestBuilder
{
  public static CreateRecipeRequestFormData Build(IFormFile? formFile = null)
  {
    var step = 1;

    return new Faker<CreateRecipeRequestFormData>()
      .RuleFor(recipe => recipe.Image, _ => formFile)
      .RuleFor(recipe => recipe.Title, f => f.Lorem.Word())
      .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
      .RuleFor(recipe => recipe.Difficulty, f => f.PickRandom<Difficulty>())
      .RuleFor(recipe => recipe.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
      .RuleFor(recipe => recipe.DishTypes, f => f.Make(3, () => f.PickRandom<DishType>()))
      .RuleFor(recipe => recipe.Instructions, f => f.Make(3, () => new InstructionRequest
      {
        Text = f.Lorem.Text().ClampLength(0, 255),
        Step = step++
      }));
  }
}
