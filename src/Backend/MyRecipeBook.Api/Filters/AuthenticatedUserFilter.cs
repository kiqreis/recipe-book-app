using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using System.Net;

namespace MyRecipeBook.Api.Filters;

public class AuthenticatedUserFilter(IAccessTokenValidator accessToken, IUserReadOnlyRepository userReadOnlyRepository) : IAsyncAuthorizationFilter
{
  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    try
    {
      var token = TokenOnRequest(context);

      var userId = accessToken.ValidateAndGetUserIdentifier(token);

      var exists = await userReadOnlyRepository.IsActiveUserWithId(userId);

      if (exists.IsFalse())
      {
        throw new UnauthorizedException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
      }
    }
    catch (MyRecipeBookException e)
    {
      context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      context.Result = new UnauthorizedObjectResult(new ErrorResponse(e.Message));
    }
    catch (SecurityTokenExpiredException)
    {
      context.Result = new UnauthorizedObjectResult(new ErrorResponse("The token is expired")
      {
        TokenIsExpired = true
      });
    }
    catch
    {
      context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
    }
  }

  private static string TokenOnRequest(AuthorizationFilterContext context)
  {
    var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

    if (string.IsNullOrWhiteSpace(authentication))
    {
      throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);
    }

    return authentication["Bearer ".Length..].Trim();
  }
}
