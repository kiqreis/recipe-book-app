using Bogus;
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
      .RuleFor(r => r.Image, _ => formFile)
      .RuleFor(r => r.Title, f => f.Lorem.Word())
      .RuleFor(r => r.CookingTime, f => f.PickRandom<CookingTime>())
      .RuleFor(r => r.Difficulty, f => f.PickRandom<Difficulty>())
      .RuleFor(r => r.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
      .RuleFor(r => r.DishTypes, f => f.Make(3, () => f.PickRandom<DishType>()))
      .RuleFor(r => r.Instructions, f => f.Make(3, () => new InstructionRequest
      {
        Text = f.Lorem.Text(),
        Step = step++
      }));
  }
}
