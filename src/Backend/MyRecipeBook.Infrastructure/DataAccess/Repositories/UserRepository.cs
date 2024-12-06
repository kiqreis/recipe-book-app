using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.UserRepository;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
  public async Task Add(User user) => await context.Users.AddAsync(user);

  public async Task<bool> IsActiveUserWithEmail(string email) => await context
    .Users.AnyAsync(user => user.Email.Equals(email) && user.IsActive);

  public async Task<User?> GetByEmailAndPassword(string email, string password)
  {
    return await context
      .Users
      .AsNoTracking()
      .FirstOrDefaultAsync(user => user.IsActive && user.Email.Equals(email) && user.Password.Equals(password));
  }

  public async Task<bool> IsActiveUserWithIdentifier(Guid userId)
  {
    return await context.Users.AnyAsync(user => user.UserId.Equals(userId) && user.IsActive);
  }

  public async Task<User> GetById(long id)
  {
    return await context.Users.FirstAsync(user => user.Id == id);
  }

  public void Update(User user) => context.Users.Update(user);
}