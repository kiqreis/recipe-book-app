using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.UserRepository;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository(AppDbContext context) : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository, IUserDeleteOnlyRepository
{
  public async Task Add(User user) => await context.Users.AddAsync(user);

  public async Task<bool> IsActiveUserWithEmail(string email) => await context
    .Users.AnyAsync(user => user.Email.Equals(email) && user.IsActive);

  public async Task<User> GetById(long id)
  {
    return await context.Users.FirstAsync(user => user.Id == id);
  }

  public void Update(User user) => context.Users.Update(user);

  public async Task<bool> IsActiveUserWithId(Guid userId) => await context.Users.AnyAsync(user => user.UserId.Equals(userId) && user.IsActive);

  public async Task<User?> GetByEmail(string email)
  {
    return await context.Users
      .AsNoTracking()
      .FirstOrDefaultAsync(user => user.IsActive && user.Email.Equals(email));
  }

  public async Task DeleteAccount(Guid userId)
  {
    var user = await context.Users.FirstOrDefaultAsync(user => user.UserId == userId);

    if (user == null)
    {
      return;
    }

    var recipes = context.Recipes.Where(recipe => recipe.UserId == user.Id);

    context.Recipes.RemoveRange(recipes);
    context.Users.Remove(user);
  }

  public async Task<User?> GetByEmailAndPassword(string email, string password)
  {
    return await context.Users
      .AsNoTracking()
      .FirstOrDefaultAsync(user => user.IsActive && user.Email.Equals(email) && user.Password.Equals(password));
  }
}