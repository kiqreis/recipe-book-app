using AutoMapper;
using MyRecipeBook.Application.SecurityConfig;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Create;

public static class CreateUser
{
  public static CreateUserResponse Execute(CreateUserRequest request, IMapper mapper, PasswordEncrypt encrypt)
  {
    var response = mapper.Map<User>(request);
    response.Password = encrypt.Encrypt(request.Password);
    
    Validate(request);
    
    return new CreateUserResponse
    {
      Name = response.Name,
      Email = response.Email
    };
  }

  private static void Validate(CreateUserRequest request)
  {
    var validator = new CreateUserValidator();
    var result = validator.Validate(request);

    if (result.IsValid == false)
    {
      var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

      throw new RequestValidationException(errorMessages);
    }
  }
}