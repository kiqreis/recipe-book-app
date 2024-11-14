using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Infrastructure.Services.LoggedUser;

public class LoggedUser(AppDbContext context, ITokenProvider tokenProvider) : ILoggedUser
{
  public async Task<User> User()
  {
    var token = tokenProvider.GetValue();
    var tokenHandler = new JwtSecurityTokenHandler();
    var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
    var userId = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

    return await context.Users.AsNoTracking()
      .FirstAsync(user => user.IsActive && user.UserId == Guid.Parse(userId));
  }
}
