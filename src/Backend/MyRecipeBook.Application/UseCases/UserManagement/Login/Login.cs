using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Login;

public class Login(IUserRepository repository, IPasswordEncrypt encrypt, IAccessTokenGenerator accessToken) : ILogin
{
  public async Task<CreateUserResponse> Execute(RequestLogin request)
  {
    var passwordEncrypt = encrypt.Encrypt(request.Password);
    var user = await repository.GetByEmailAndPassword(request.Email, passwordEncrypt) ?? throw new InvalidLoginException();

    return new CreateUserResponse
    {
      Name = user.Name,
      Token = new TokenResponse
      {
        AccessToken = accessToken.Generate(user.UserId)
      }
    };
  }
}
