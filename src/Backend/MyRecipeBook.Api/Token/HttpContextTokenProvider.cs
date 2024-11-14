using MyRecipeBook.Domain.Security.Token;

namespace MyRecipeBook.Api.Token;

public class HttpContextTokenProvider(IHttpContextAccessor contextAccessor) : ITokenProvider
{
  public string GetValue()
  {
    var authentication = contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

    return authentication["Bearer ".Length..].Trim();
  }
}
