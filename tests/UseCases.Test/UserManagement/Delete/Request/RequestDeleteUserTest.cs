using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.ServiceBus;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Delete.Request;
using MyRecipeBook.Domain.Entities;

namespace UseCases.Test.UserManagement.Delete.Request;

public class RequestDeleteUserTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var useCase = RequestDeleteUser(user);
    var action = async () => await useCase.Execute();

    await action.Should().NotThrowAsync();

    user.IsActive.Should().BeFalse();
  }


  private static RequestDeleteUser RequestDeleteUser(User user)
  {
    var queue = DeleteUserQueueBuilder.Build();
    var unitOfWork = UnitOfWorkBuilder.Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();

    return new RequestDeleteUser(queue, repository, loggedUser, unitOfWork);
  }
}
