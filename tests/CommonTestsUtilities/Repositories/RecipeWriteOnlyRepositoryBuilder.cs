using Moq;
using MyRecipeBook.Domain.Repositories.RecipeRepository;

namespace CommonTestsUtilities.Repositories;

public class RecipeWriteOnlyRepositoryBuilder
{
  public static IRecipeWriteOnlyRepository Build()
  {
    var mock = new Mock<IRecipeWriteOnlyRepository>();

    return mock.Object;
  }
}
