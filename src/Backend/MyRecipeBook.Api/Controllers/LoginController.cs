using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.UserManagement.Login;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

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
}
