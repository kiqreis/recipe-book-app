namespace MyRecipeBook.Domain.Security.Encryption;

public interface IPasswordEncrypt
{
  public string Encrypt(string password);
}
