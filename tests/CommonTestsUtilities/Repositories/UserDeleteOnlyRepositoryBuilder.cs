using Moq;
using MyRecipeBook.Domain.Repositories.UserRepository;

namespace CommonTestsUtilities.Repositories;

public class UserDeleteOnlyRepositoryBuilder
{
  public static IUserDeleteOnlyRepository Build()
  {
    var mock = new Mock<IUserDeleteOnlyRepository>();

    return mock.Object;
  }
}
