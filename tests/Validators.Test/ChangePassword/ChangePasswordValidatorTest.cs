using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;
using MyRecipeBook.Exceptions;

namespace Validators.Test.ChangePassword;

public class ChangePasswordValidatorTest
{
  [Fact]
  public void Success()
  {
    var validator = new ChangePasswordValidator();
    var request = ChangePasswordRequestBuilder.Build();
    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(5)]
  public void Error_Password_Invalid(int passwordLength)
  {
    var validator = new ChangePasswordValidator();
    var request = ChangePasswordRequestBuilder.Build(passwordLength);
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_INVALID));
  }

  [Fact]
  public void Error_Password_Empty()
  {
    var validator = new ChangePasswordValidator();
    var request = ChangePasswordRequestBuilder.Build();

    request.NewPassword = string.Empty;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle()
      .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_EMPTY));
  }
}
