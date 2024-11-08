namespace MyRecipeBook.Domain.Security.Token;

public interface IAccessTokenGenerator
{
  public string Generate(Guid userId);
}
