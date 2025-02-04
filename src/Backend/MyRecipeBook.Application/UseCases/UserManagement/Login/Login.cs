using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Login;

public class Login(IUserReadOnlyRepository userReadOnlyRepository, IPasswordEncrypt encrypt, IAccessTokenGenerator accessToken) : ILogin
{
  public async Task<CreateUserResponse> Execute(RequestLogin request)
  {
    var user = await userReadOnlyRepository.GetByEmail(request.Email);

    if (user == null || encrypt.IsValid(request.Password, user.Password).IsFalse())
    {
      throw new InvalidLoginException();
    }

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
