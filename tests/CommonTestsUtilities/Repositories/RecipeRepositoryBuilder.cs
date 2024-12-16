using Moq;
using MyRecipeBook.Domain.Repositories.RecipeRepository;

namespace CommonTestsUtilities.Repositories;

public class RecipeRepositoryBuilder
{
  public static IRecipeRepository Build()
  {
    var mock = new Mock<IRecipeRepository>();

    return mock.Object;
  }
}
