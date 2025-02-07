using Moq;
using MyRecipeBook.Domain.Repositories.TokenRepository;

namespace CommonTestsUtilities.Repositories;

public class TokenRepositoryBuilder
{
  public static ITokenRepository Build()
  {
    var mock = new Mock<ITokenRepository>();

    return mock.Object;
  }
}
