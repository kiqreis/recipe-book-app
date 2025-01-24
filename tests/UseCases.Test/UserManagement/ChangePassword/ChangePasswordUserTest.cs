using CommonTestsUtilities.Encrypt;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

public class ChangePasswordUserTest
{
  [Fact]
  public async Task Success()
  {
    (var user, var password) = UserBuilder.Build();

    var request = ChangePasswordRequestBuilder.Build();

    request.Password = password;

    var useCase = ChangePassword(user);

    Func<Task> action = async () => await useCase.Execute(request);

    await action.Should().NotThrowAsync();

    var passwordEncrypt = PasswordEncryptBuilder.Build();

    user.Password.Should().Be(passwordEncrypt.Encrypt(request.NewPassword));
  }

  [Fact]
  public async Task Error_NewPassword_Empty()
  {
    (var user, var password) = UserBuilder.Build();

    var request = new ChangePasswordRequest
    {
      Password = password,
      NewPassword = string.Empty
    };

    var useCase = ChangePassword(user);

    Func<Task> action = async () => await useCase.Execute(request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.PASSWORD_EMPTY));

    var passwordEncrypt = PasswordEncryptBuilder.Build();

    user.Password.Should().Be(passwordEncrypt.Encrypt(password));
  }

  [Fact]
  public async Task Error_Different_Password_From_The_Current()
  {
    (var user, var password) = UserBuilder.Build();
    var request = ChangePasswordRequestBuilder.Build();
    var useCase = ChangePassword(user);

    Func<Task> action = async () => await useCase.Execute(request);

    await action.Should().ThrowAsync<RequestValidationException>()
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.DIFFERENT_PASSWORD_FROM_THE_CURRENT));

    var passwordEncrypt = PasswordEncryptBuilder.Build();

    user.Password.Should().Be(passwordEncrypt.Encrypt(password));
  }

  private static ChangePassword ChangePassword(User user)
  {
    var unitOfWork = UnityOfWorkBuilder.Build();
    var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var passwordEncrypt = PasswordEncryptBuilder.Build();

    return new ChangePassword(loggedUser, passwordEncrypt, userUpdateOnlyRepository, unitOfWork);
  }
}
