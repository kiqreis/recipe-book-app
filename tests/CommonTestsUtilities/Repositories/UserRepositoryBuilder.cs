using Moq;
using MyRecipeBook.Domain.Repositories.UserRepository;

namespace CommonTestsUtilities.Repositories;

public class UserRepositoryBuilder
{
  public static IUserRepository Build()
  {
    var mock = new Mock<IUserRepository>();

    return mock.Object;
  }
}
