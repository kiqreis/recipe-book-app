using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.UserManagement.ExternalLogin;
using MyRecipeBook.Application.UseCases.UserManagement.Login;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using System.Security.Claims;

namespace MyRecipeBook.Api.Controllers;

public class LoginController : MyRecipeBookControllerBase
{
  [HttpPost]
  [ProducesResponseType<CreateUserResponse>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Login(RequestLogin request, [FromServices] ILogin login)
  {
    var response = await login.Execute(request);

    return Ok(response);
  }

  [HttpGet]
  [Route("google")]
  public async Task<IActionResult> LoginGoogle(string url, [FromServices] IExternalLogin externalLogin)
  {
    var authenticate = await Request.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

    if (IsNotAuthenticated(authenticate))
    {
      return Challenge(GoogleDefaults.AuthenticationScheme);
    }
    else
    {
      var claims = authenticate.Principal!.Identities.First().Claims;

      var name = claims.First(claim => claim.Type == ClaimTypes.Name).Value;
      var email = claims.First(claim => claim.Type == ClaimTypes.Email).Value;

      var token = await externalLogin.Execute(name, email);

      return Redirect($"{url}/{token}");
    }
  }
}
