using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration builder)
  {
    AddAppDbContext(services, builder);
    AddRepositories(services);
    AddFluentMigrator(services, builder);
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

  private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.ConnectionString();

    services.AddFluentMigratorCore().ConfigureRunner(opt =>
    {
      opt.AddSqlServer()
      .WithGlobalConnectionString(connectionString)
      .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
    });
  }
}