using Bogus;
using CommonTestsUtilities.Encrypt;
using MyRecipeBook.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public class UserBuilder
{
  public static (User user, string password) Build()
  {
    var passwordEncrypt = PasswordEncryptBuilder.Build();
    var password = new Faker().Internet.Password();

    var user = new Faker<User>()
      .RuleFor(user => user.Id, () => 1)
      .RuleFor(user => user.Name, (f) => f.Person.FirstName)
      .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
      .RuleFor(user => user.UserId, _ => Guid.NewGuid())
      .RuleFor(user => user.Password, () => passwordEncrypt.Encrypt(password));

    return (user, password);
  }
}
