using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
  public static void AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration builder)
  {
    AddAppDbContext(serviceCollection, builder);
    AddRepositories(serviceCollection);
  }

  private static void AddAppDbContext(IServiceCollection serviceCollection, IConfiguration builder)
  {
    var connectionString = builder.GetConnectionString("DefaultConnection");

    serviceCollection.AddDbContext<AppDbContext>(opt =>
    {
      opt.UseSqlServer(connectionString);
    });
  }
  
  private static void AddRepositories(IServiceCollection serviceCollection)
  {
    serviceCollection.AddScoped<IUserRepository, UserRepository>();
  }
}