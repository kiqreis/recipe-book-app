using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public class RecipeFilterRequestBuilder
{
  public static RecipeFilterRequest Build()
  {
    return new Faker<RecipeFilterRequest>()
      .RuleFor(recipe => recipe.CookingTimes, f => f.Make(1, () => f.PickRandom<CookingTime>()))
      .RuleFor(recipe => recipe.Difficulties, f => f.Make(1, () => f.PickRandom<Difficulty>()))
      .RuleFor(recipe => recipe.DishTypes, f => f.Make(1, () => f.PickRandom<DishType>()))
      .RuleFor(recipe => recipe.RecipeTitleIngredient, f => f.Lorem.Word());
  }
}
