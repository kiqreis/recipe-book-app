using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public class LoginUserRequestBuilder
{
  public static RequestLogin Build()
  {
    return new Faker<RequestLogin>()
      .RuleFor(user => user.Email, (f) => f.Internet.Email())
      .RuleFor(user => user.Password, (f) => f.Internet.Password());
  }
}
