using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Attributes;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Api.Controllers;

[AuthenticateUser]
public class UserController : MyRecipeBookControllerBase
{
  [HttpPost]
  [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Create(CreateUserRequest request, [FromServices] ICreateUser createUser)
  {
    return Created(string.Empty, await createUser.Execute(request));
  }
}