using MyRecipeBook.Application.SecurityConfig;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Login;

public class Login(IUserRepository repository, PasswordEncrypt encrypt) : ILogin
{
  public async Task<CreateUserResponse> Execute(RequestLogin request)
  {
    var passwordEncrypt = encrypt.Encrypt(request.Password);
    var user = await repository.GetByEmailAndPassword(request.Email, passwordEncrypt) ?? throw new InvalidLoginException();

    return new CreateUserResponse
    {
      Name = user.Name,
    };
  }
}
