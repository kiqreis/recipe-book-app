using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.UserRepository;

public interface IUserReadOnlyRepository
{
  Task<bool> IsActiveUserWithEmail(string email);
  Task<bool> IsActiveUserWithId(Guid userId);
  Task<User?> GetByEmail(string email);
}
