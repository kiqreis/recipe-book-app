namespace MyRecipeBook.Domain.Repositories.UserRepository;

public interface IUserDeleteOnlyRepository
{
  Task DeleteAccount(Guid userId);
}
