using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Attributes;
using MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Application.UseCases.UserManagement.Delete.Request;
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
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  [AuthenticateUser]
  public async Task<IActionResult> Update([FromServices] IUpdateUser updateUser, UpdateUserRequest request)
  {
    await updateUser.Execute(request);

    return NoContent();
  }

  [HttpPut("change-password")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  [AuthenticateUser]
  public async Task<IActionResult> ChangePassword([FromServices] IChangePassword changePassword, ChangePasswordRequest request)
  {
    await changePassword.Execute(request);

    return NoContent();
  }

  [HttpDelete]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> Delete([FromServices] IRequestDeleteUser deleteUser)
  {
    await deleteUser.Execute();
    
    return NoContent();
  }
} 