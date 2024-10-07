using CommonTestsUtilities.Requests;
using MyRecipeBook.Application.UseCases.UserManagement.Create;

namespace Validators.Test.UserTests.Create;

public class CreateUserValidatorTest
{
  [Fact]
  public void Success()
  {
    var validator = new CreateUserValidator();
    var request = CreateUserRequestBuilder.Build();
    var result = validator.Validate(request);

    Assert.True(result.IsValid);
  }
}
