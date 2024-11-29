using MyRecipeBook.Domain.Security.Encryption;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Encrypt;

public class Sha512PasswordEncrypt : IPasswordEncrypt
{
  public string Encrypt(string password)
  {
    var bytes = Encoding.ASCII.GetBytes($"{password}qQuLjh6JAws497M");
    var hashBytes = SHA512.HashData(bytes);

    return StringBytes(hashBytes);
  }

  private static string StringBytes(byte[] bytes)
  {
    var sb = new StringBuilder();

    foreach (byte b in bytes)
    {
      var hex = b.ToString("x2");
      sb.Append(hex);
    }

    return sb.ToString();
  }
}
