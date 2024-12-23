using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public class RecipeFilterRequestBuilder
{
  public static RecipeFilterRequest Build()
  {
    return new Faker<RecipeFilterRequest>()
      .RuleFor(user => user.CookingTimes, f => f.Make(1, () => f.PickRandom<CookingTime>()))
      .RuleFor(user => user.Difficulties, f => f.Make(1, () => f.PickRandom<Difficulty>()))
      .RuleFor(user => user.DishTypes, f => f.Make(1, () => f.PickRandom<DishType>()))
      .RuleFor(user => user.RecipeTitleIngredient, f => f.Lorem.Word());
  }
}
