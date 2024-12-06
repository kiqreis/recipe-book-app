using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public class ChangePasswordRequestBuilder
{
  public static ChangePasswordRequest Build(int passwordLength = 10)
  {
    return new Faker<ChangePasswordRequest>()
      .RuleFor(u => u.Password, f => f.Internet.Password())
      .RuleFor(u => u.NewPassword, f => f.Internet.Password(passwordLength));
  }
}
