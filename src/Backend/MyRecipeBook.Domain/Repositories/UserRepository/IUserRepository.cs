using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.UserRepository;

public interface IUserRepository
{
  public Task Add(User user);
  public Task<bool> IsActiveUserWithEmail(string email);
  public Task<User?> GetByEmailAndPassword(string email, string password);
}