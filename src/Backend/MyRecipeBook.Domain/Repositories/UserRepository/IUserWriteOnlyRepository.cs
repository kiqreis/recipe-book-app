using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.UserRepository;

public interface IUserWriteOnlyRepository
{
  Task Add(User user);
}
