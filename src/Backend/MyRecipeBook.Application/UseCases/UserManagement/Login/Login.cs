using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.TokenRepository;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Login;

public class Login(IUserReadOnlyRepository userReadOnlyRepository, IPasswordEncrypt encrypt, IAccessTokenGenerator accessToken, IRefreshTokenGenerator refreshTokenGenerator, ITokenRepository tokenRepository, IUnitOfWork unitOfWork) : ILogin
{
  public async Task<CreateUserResponse> Execute(RequestLogin request)
  {
    var user = await userReadOnlyRepository.GetByEmail(request.Email);

    if (user == null || encrypt.IsValid(request.Password, user.Password).IsFalse())
    {
      throw new InvalidLoginException();
    }

    var refreshToken = await CreateAndSaveRefreshToken(user);

    return new CreateUserResponse
    {
      Name = user.Name,
      Tokens = new TokensResponse
      {
        AccessToken = accessToken.Generate(user.UserId),
        RefreshToken = refreshToken
      }
    };
  }

  private async Task<string> CreateAndSaveRefreshToken(User user)
  {
    var refreshToken = new RefreshToken
    {
      Value = refreshTokenGenerator.Generate(),
      UserId = user.Id
    };

    await tokenRepository.SaveNewRefreshToken(refreshToken);
    await unitOfWork.CommitAsync();

    return refreshToken.Value;
  }
}
