using Moq;
using MyRecipeBook.Domain.Security.Token;

namespace CommonTestsUtilities.Token;

public class RefreshTokenGeneratorBuilder
{
  private readonly Mock<IRefreshTokenGenerator> _repository;

  public RefreshTokenGeneratorBuilder() => _repository = new Mock<IRefreshTokenGenerator>();

  public IRefreshTokenGenerator Build() => _repository.Object;
}
