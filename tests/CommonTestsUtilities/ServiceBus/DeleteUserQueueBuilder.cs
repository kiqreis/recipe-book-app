using Moq;
using MyRecipeBook.Domain.ServiceBus;

namespace CommonTestsUtilities.ServiceBus;

public class DeleteUserQueueBuilder
{
  public static IDeleteUserQueue Build()
  {
    return new Mock<IDeleteUserQueue>().Object;
  }
}
