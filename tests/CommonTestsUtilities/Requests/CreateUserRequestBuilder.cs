using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public class CreateUserRequestBuilder
{
  public static CreateUserRequest Build(int passwordLength = 10)
  {
    return new Faker<CreateUserRequest>()
      .RuleFor(user => user.Name, (f) => f.Person.FirstName)
      .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
      .RuleFor(user => user.Password, (f) => f.Internet.Password(passwordLength));
  }
}
