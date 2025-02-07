using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using System.Reflection;

namespace MyRecipeBook.Infrastructure.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> context) : DbContext(context)
{
  public DbSet<User> Users { get; set; }

  public DbSet<Recipe> Recipes { get; set; }

  public DbSet<RefreshToken> RefreshTokens { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}