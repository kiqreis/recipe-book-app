using MyRecipeBook.Domain.Security.Encryption;

namespace MyRecipeBook.Infrastructure.Security.Encrypt;

public class BCryptNet : IPasswordEncrypt
{
  public string Encrypt(string password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password);
  }

  public bool IsValid(string password, string passwordHash)
  {
    return BCrypt.Net.BCrypt.Verify(password, passwordHash);
  }
}
