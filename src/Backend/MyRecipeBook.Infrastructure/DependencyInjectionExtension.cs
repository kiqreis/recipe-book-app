using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration builder)
  {
    AddAppDbContext(services, builder);
    AddRepositories(services);
  }

  private static void AddAppDbContext(IServiceCollection services, IConfiguration builder)
  {
    var connectionString = builder.GetConnectionString("DefaultConnection");

    services.AddDbContext<AppDbContext>(opt =>
    {
      opt.UseSqlServer(connectionString);
    });
  }

  private static void AddRepositories(IServiceCollection services)
  {
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IUserRepository, UserRepository>();
  }
}