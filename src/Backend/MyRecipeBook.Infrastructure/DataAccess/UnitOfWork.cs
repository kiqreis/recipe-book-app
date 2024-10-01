using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
  public async Task CommitAsync() => await context.SaveChangesAsync();

  public void Dispose() => context.Dispose();
}