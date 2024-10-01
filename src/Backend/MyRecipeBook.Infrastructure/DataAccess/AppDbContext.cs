using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using System.Reflection;

namespace MyRecipeBook.Infrastructure.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> context) : DbContext(context)
{
  public DbSet<User> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}