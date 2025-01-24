using Moq;
using MyRecipeBook.Domain.Repositories.UserRepository;

namespace CommonTestsUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
  public static IUserWriteOnlyRepository Build()
  {
    var mock = new Mock<IUserWriteOnlyRepository>();

    return mock.Object;
  }
}
