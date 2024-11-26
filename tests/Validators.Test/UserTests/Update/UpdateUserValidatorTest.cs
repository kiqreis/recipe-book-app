﻿using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Update;
using MyRecipeBook.Exceptions;

namespace Validators.Test.UserTests.Update;

public class UpdateUserValidatorTest
{
  [Fact]
  public void Success()
  {
    var validator = new UpdateUserValidator();
    var request = UpdateUserRequestBuilder.Build();
    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Error_Name_Empty()
  {
    var validator = new UpdateUserValidator();
    var request = UpdateUserRequestBuilder.Build();

    request.Name = string.Empty;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
  }

  [Fact]
  public void Error_Email_Empty()
  {
    var validator = new UpdateUserValidator();
    var request = UpdateUserRequestBuilder.Build();

    request.Email = string.Empty;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
  }

  [Fact]
  public void Error_Email_Invalid()
  {
    var validator = new UpdateUserValidator();
    var request = UpdateUserRequestBuilder.Build();

    request.Email = "email.com";

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
  }
}
