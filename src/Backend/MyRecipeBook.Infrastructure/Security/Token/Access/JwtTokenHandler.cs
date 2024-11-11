using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Token.Access;

public abstract class JwtTokenHandler
{
  protected SymmetricSecurityKey SecurityKey(string signingKey)
  {
    var bytes = Encoding.UTF8.GetBytes(signingKey);

    return new SymmetricSecurityKey(bytes);
  }
}
