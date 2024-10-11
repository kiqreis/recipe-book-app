using Moq;
using MyRecipeBook.Domain.Repositories.UserRepository;

namespace CommonTestsUtilities.Repositories;

public class UserRepositoryBuilder
{
  private readonly Mock<IUserRepository> _repository;

  public UserRepositoryBuilder() => _repository = new Mock<IUserRepository>();

  public IUserRepository Build() => _repository.Object;
  
  public void IsActiveUserWithEmail(string email)
  {
    _repository.Setup(repository => repository.IsActiveUserWithEmail(email))
      .ReturnsAsync(true);
  } 
}
