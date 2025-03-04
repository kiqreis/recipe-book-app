﻿using CommonTestsUtilities.Encrypt;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Login;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using MyRecipeBook.Communication.Requests;
using CommonTestsUtilities.Token;

public class LoginUserTest
{
  [Fact]
  public async Task Success()
  {
    (var user, var password) = UserBuilder.Build();
    var useCase = LoginUser(user);

    var result = await useCase.Execute(new RequestLogin
    {
      Email = user.Email,
      Password = password
    });

    result.Should().NotBeNull();
    result.Tokens.Should().NotBeNull();
    result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
    result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
  }

  [Fact]
  public async Task Error_Invalid_User()
  {
    var request = LoginUserRequestBuilder.Build();
    var useCase = LoginUser();

    Func<Task> action = async () => await useCase.Execute(request);

    await action.Should().ThrowAsync<InvalidLoginException>()
      .Where(e => e.Message.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
  }

  private static Login LoginUser(User? user = null)
  {
    var passwordEncrypt = PasswordEncryptBuilder.Build();
    var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
    var accessToken = JwtTokenGeneratorBuilder.Build();
    var tokenRepository = TokenRepositoryBuilder.Build();
    var refreshToken = new RefreshTokenGeneratorBuilder();
    var unitOfWork = UnitOfWorkBuilder.Build();

    if (user != null)
    {
      userReadOnlyRepository.GetByEmailAndPassword(user);
    }

    return new Login(userReadOnlyRepository.Build(), passwordEncrypt, accessToken, refreshToken.Build(), tokenRepository, unitOfWork);
  }
}
