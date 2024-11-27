using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Update;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.UserManagement.Update;

public class UpdateUserTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var request = UpdateUserRequestBuilder.Build();
    var useCase = UpdateUser(user);

    Func<Task> action = async () => await useCase.Execute(request);

    await action.Should().NotThrowAsync();

    user.Name.Should().Be(request.Name);
    user.Email.Should().Be(request.Email);
  }

  [Fact]
  public async Task Error_Name_Empty()
  {
    (var user, _) = UserBuilder.Build();
    var request = UpdateUserRequestBuilder.Build();

    request.Name = string.Empty;

    var useCase = UpdateUser(user);

    Func<Task> action = async () => await useCase.Execute(request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));

    user.Name.Should().NotBe(request.Name);
    user.Email.Should().NotBe(request.Email);
  }

  [Fact]
  public async Task Error_Email_Already_Exists()
  {
    (var user, _) = UserBuilder.Build();
    var request = UpdateUserRequestBuilder.Build();
    var useCase = UpdateUser(user, request.Email);

    Func<Task> action = async () => await useCase.Execute(request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_EXISTS));

    user.Name.Should().NotBe(request.Name);
    user.Email.Should().NotBe(request.Email);
  }

  private static UpdateUser UpdateUser(User user, string? email = null)
  {
    var unitOfWork = UnityOfWorkBuilder.Build();
    var repository = new UserRepositoryBuilder().GetById(user).Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var userRepository = new UserRepositoryBuilder();

    if (string.IsNullOrEmpty(email) == false)
    {
      userRepository.IsActiveUserWithEmail(email!);
    }

    return new UpdateUser(loggedUser, repository, unitOfWork);
  }
}
