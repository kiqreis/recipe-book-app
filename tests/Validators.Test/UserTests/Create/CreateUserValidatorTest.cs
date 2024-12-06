using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Exceptions;

namespace Validators.Test.UserTests.Create;

public class CreateUserValidatorTest
{
  [Fact]
  public void Success()
  {
    var validator = new CreateUserValidator();
    var request = CreateUserRequestBuilder.Build();
    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Error_Empty_Name()
  {
    var validator = new CreateUserValidator();
    var request = CreateUserRequestBuilder.Build();

    request.Name = string.Empty;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
  }

  [Fact]
  public void Error_Empty_Email()
  {
    var validator = new CreateUserValidator();
    var request = CreateUserRequestBuilder.Build();

    request.Email = string.Empty;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(5)]
  public void Error_Password_Invalid(int passwordLength)
  {
    var validator = new CreateUserValidator();
    var request = CreateUserRequestBuilder.Build(passwordLength);
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_INVALID));
  }

  [Fact]
  public void Error_Password_Empty()
  {
    var validator = new CreateUserValidator();
    var request = CreateUserRequestBuilder.Build();

    request.Password = string.Empty;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_EMPTY));
  }
}
