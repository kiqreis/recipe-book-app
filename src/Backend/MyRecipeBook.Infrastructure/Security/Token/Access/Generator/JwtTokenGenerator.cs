using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Infrastructure.Security.Token.Access.Generator;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey) : JwtTokenHandler, IAccessTokenGenerator
{
  public string Generate(Guid userId)
  {
    var claims = new List<Claim>()
    {
      new(ClaimTypes.Sid, userId.ToString())
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddMinutes(expirationTimeMinutes),
      SigningCredentials = new SigningCredentials(SecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var securityToken = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(securityToken);
  }
}
