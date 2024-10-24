using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.UserRepository;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
  public async Task Add(User user)
  {
    await context.Users.AddAsync(user);
    await context.SaveChangesAsync();
  }

  public async Task<bool> IsActiveUserWithEmail(string email) => await context
    .Users.AnyAsync(u => u.Email.Equals(email) && u.IsActive);

  public async Task<User?> GetByEmailAndPassword(string email, string password) => await context.Users.AsNoTracking()
    .FirstOrDefaultAsync(user => user.IsActive && user.Email.Equals(email) && user.Password.Equals(password));
}