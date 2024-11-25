using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.UserRepository;

public interface IUserRepository
{
  public Task Add(User user);
  public Task<bool> IsActiveUserWithEmail(string email);
  public Task<User?> GetByEmailAndPassword(string email, string password);
  public Task<bool> IsActiveUserWithIdentifier(Guid userId);
  public Task<User> GetById(long id);
  public void Update(User user);
}