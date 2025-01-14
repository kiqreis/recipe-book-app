using Bogus;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;

namespace CommonTestsUtilities.Dtos;

public class RecipeGeneratedDtoBuilder
{
  public static RecipeGeneratedDto Build()
  {
    return new Faker<RecipeGeneratedDto>()
      .RuleFor(recipe => recipe.Title, f => f.Lorem.Word())
      .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
      .RuleFor(recipe => recipe.Ingredients, f => f.Make(1, () => f.Commerce.ProductName()))
      .RuleFor(recipe => recipe.Instructions, f => f.Make(1, () => new InstructionGeneratedDto
      {
        Step = 1,
        Text = f.Lorem.Text()
      }));
  }
}
