﻿using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Update;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
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
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.NAME_EMPTY));

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
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_ALREADY_EXISTS));

    user.Name.Should().NotBe(request.Name);
    user.Email.Should().NotBe(request.Email);
  }

  private static UpdateUser UpdateUser(User user, string? email = null)
  {
    var unitOfWork = UnitOfWorkBuilder.Build();
    var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();

    if (email.NotEmpty())
    {
      userReadOnlyRepository.IsActiveUserWithEmail(email!);
    }

    return new UpdateUser(loggedUser, userUpdateOnlyRepository, userReadOnlyRepository.Build(), unitOfWork);
  }
}
