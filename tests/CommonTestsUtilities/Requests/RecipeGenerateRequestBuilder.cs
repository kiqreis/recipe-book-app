using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public class RecipeGenerateRequestBuilder
{
  public static RecipeGeneratedRequest Build(int count = 6)
  {
    return new Faker<RecipeGeneratedRequest>()
    .RuleFor(user => user.Ingredients, f => f.Make(count, () => f.Commerce.ProductName()));
  }
}

