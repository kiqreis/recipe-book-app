using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Infrastructure.Security.Token.Access.Validator;

public class JwtTokenValidator(string signingKey) : JwtTokenHandler, IAccessTokenValidator
{
  public Guid ValidateAndGetUserIdentifier(string token)
  {
    var validationParameter = new TokenValidationParameters
    {
      ValidateAudience = false,
      ValidateIssuer = false,
      IssuerSigningKey = SecurityKey(signingKey),
      ClockSkew = new TimeSpan(0)
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var principal = tokenHandler.ValidateToken(token, validationParameter, out _);

    var userId = principal.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

    return Guid.Parse(userId);
  }
}
