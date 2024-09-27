using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.SecurityConfig;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(IMapper mapper, PasswordEncrypt encrypt) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<CreateUserResponse>(StatusCodes.Status400BadRequest)]
    public IActionResult Create(CreateUserRequest request)
    {
        return Created(string.Empty, CreateUser.Execute(request, mapper, encrypt));
    }
}