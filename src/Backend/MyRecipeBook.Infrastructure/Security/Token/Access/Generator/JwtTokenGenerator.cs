using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace MyRecipeBook.Infrastructure.Security.Token.Access.Generator;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey) : IAccessTokenGenerator
{
  private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
  private readonly string _signingKey = signingKey;

  public string Generate(Guid userId)
  {
    var claims = new List<Claim>()
    {
      new(ClaimTypes.Sid, userId.ToString())
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
      SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var securityToken = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(securityToken);
  }

  private SymmetricSecurityKey SecurityKey()
  {
    var bytes = Encoding.UTF8.GetBytes(_signingKey);
    return new SymmetricSecurityKey(bytes);
  }
}
