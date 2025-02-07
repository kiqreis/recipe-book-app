using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.TokenManagement.RefreshToken;

public interface IUseRefreshToken
{
  Task<TokensResponse> Execute(NewTokenRequest request);
}
