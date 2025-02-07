using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.TokenRepository;

public interface ITokenRepository
{
  Task<RefreshToken?> Get(string refreshToken);
  Task SaveNewRefreshToken(RefreshToken refreshToken);
}
