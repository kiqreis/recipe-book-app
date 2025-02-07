using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.TokenRepository;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.TokenManagement.RefreshToken;

public class UseRefreshToken(ITokenRepository tokenRepository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator) : IUseRefreshToken
{
  public async Task<TokensResponse> Execute(NewTokenRequest request)
  {
    var refreshToken = await tokenRepository.Get(request.RefreshToken);

    if (refreshToken == null)
    {
      throw new RefreshTokenNotFoundException();
    }

    var refreshTokenValidUntil = refreshToken.CreatedAt.AddMinutes(MyRecipeBookRuleConstants.REFRESH_TOKEN_EXPIRATION_MINUTES);

    if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
    {
      throw new RefreshTokenExpiredException();
    }

    var newRefreshToken = new Domain.Entities.RefreshToken
    {
      Value = refreshTokenGenerator.Generate(),
      UserId = refreshToken.UserId
    };

    await tokenRepository.SaveNewRefreshToken(newRefreshToken);
    await unitOfWork.CommitAsync();

    return new TokensResponse
    {
      AccessToken = accessTokenGenerator.Generate(refreshToken.User.UserId),
      RefreshToken = newRefreshToken.Value
    };
  }
}
