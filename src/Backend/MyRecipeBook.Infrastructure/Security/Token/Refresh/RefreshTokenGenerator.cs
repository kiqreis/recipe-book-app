using Azure.Storage.Blobs.Models;
using MyRecipeBook.Domain.Security.Token;

namespace MyRecipeBook.Infrastructure.Security.Token.Refresh;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
  public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}
