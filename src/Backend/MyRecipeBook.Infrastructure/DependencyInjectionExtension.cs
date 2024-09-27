using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
  public static void AddInfrastructure(this IServiceCollection serviceCollection)
  {
    AddAppDbContext(serviceCollection);
    AddRepositories(serviceCollection);
  }

  private static void AddAppDbContext(IServiceCollection serviceCollection)
  {
    const string connectionString = 
      "Server=localhost,1433;Database=balta;User ID=sa;Password=Avuv@s0305;Trusted_Connection=True; TrustServerCertificate=True;";

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