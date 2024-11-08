using System.Text.Json.Serialization;

namespace MyRecipeBook.Communication.Responses;

public class CreateUserResponse
{
  public string Name { get; set; } = string.Empty;

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Email { get; set; }

  public TokenResponse Token { get; set; } = default!;
}