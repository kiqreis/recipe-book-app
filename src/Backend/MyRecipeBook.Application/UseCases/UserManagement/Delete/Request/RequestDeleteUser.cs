using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.ServiceBus;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.UserManagement.Delete.Request;

public class RequestDeleteUser(IDeleteUserQueue queue, IUserRepository repository, ILoggedUser _loggedUser, IUnitOfWork unitOfWork) : IRequestDeleteUser
{
  public async Task Execute()
  {
    var loggedUser = await _loggedUser.User();
    var user = await repository.GetById(loggedUser.Id);

    user.IsActive = false;

    repository.Update(user);

    await unitOfWork.CommitAsync();

    await queue.SendMessage(loggedUser);
  }
}
