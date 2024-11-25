using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Attributes;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Application.UseCases.UserManagement.Profile;
using MyRecipeBook.Application.UseCases.UserManagement.Update;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Api.Controllers;

public class UserController : MyRecipeBookControllerBase
{
  [HttpPost]
  [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Create(CreateUserRequest request, [FromServices] ICreateUser createUser)
  {
    return Created(string.Empty, await createUser.Execute(request));
  }

  [HttpGet]
  [ProducesResponseType<UserProfileResponse>(StatusCodes.Status200OK)]
  [AuthenticateUser]
  public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfile userProfile)
  {
    var result = await userProfile.Execute();

    return Ok(result);
  }

  [HttpPut]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [AuthenticateUser]
  public async Task<IActionResult> Update([FromServices] IUpdateUser updateUser, UpdateUserRequest request)
  {
    await updateUser.Execute(request);

    return NoContent();
  }
}