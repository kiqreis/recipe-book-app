using AutoMapper;
using MyRecipeBook.Application.SecurityConfig;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Create;

public class CreateUser(IMapper mapper, PasswordEncrypt encrypt, IUserRepository repository, IUnitOfWork unitOfWork) : ICreateUser
{
  public async Task<CreateUserResponse> Execute(CreateUserRequest request)
  {
    Validate(request);

    var user = mapper.Map<User>(request);
    user.Password = encrypt.Encrypt(user.Password);

    await repository.Add(user);
    await unitOfWork.CommitAsync();

    return new CreateUserResponse
    {
      Name = user.Name,
      Email = user.Email
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