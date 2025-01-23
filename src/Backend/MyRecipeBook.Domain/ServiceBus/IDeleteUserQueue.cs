using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.ServiceBus;

public interface IDeleteUserQueue
{
  Task SendMessage(User user);
}
