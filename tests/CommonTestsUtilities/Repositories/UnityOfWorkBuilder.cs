using Moq;
using MyRecipeBook.Domain.Repositories;

namespace CommonTestsUtilities.Repositories;

public class UnityOfWorkBuilder
{
  public static IUnitOfWork Build()
  {
    var mock = new Mock<IUnitOfWork>();

    return mock.Object; 
  }
}
