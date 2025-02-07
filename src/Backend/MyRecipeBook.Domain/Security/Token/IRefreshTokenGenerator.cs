namespace MyRecipeBook.Domain.Security.Token;

public interface IRefreshTokenGenerator
{
  public string Generate();
}
