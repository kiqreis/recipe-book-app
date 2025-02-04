using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Infrastructure.Security.Encrypt;

namespace CommonTestsUtilities.Encrypt;

public class PasswordEncryptBuilder
{
  public static IPasswordEncrypt Build() => new BCryptNet();
}
