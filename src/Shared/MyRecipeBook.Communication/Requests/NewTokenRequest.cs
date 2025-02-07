namespace MyRecipeBook.Communication.Requests;

public class NewTokenRequest
{
  public string RefreshToken { get; set; } = string.Empty;
}
