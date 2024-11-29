using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Encrypt;
using MyRecipeBook.Infrastructure.Security.Token.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Token.Access.Validator;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    AddPasswordEncrypt(services);
    AddRepositories(services);
    AddLoggedUser(services);
    AddToken(services, configuration);

    if (configuration.IsUnitTestEnvironment())
    {
      return;
    }

    AddAppDbContext(services, configuration);
    AddFluentMigrator(services, configuration);
  }

  private static void AddAppDbContext(IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("DefaultConnection");

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

  private static void AddToken(IServiceCollection services, IConfiguration configuration)
  {
    var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
    var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

    services.AddScoped<IAccessTokenGenerator>(opt => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
    services.AddScoped<IAccessTokenValidator>(opt => new JwtTokenValidator(signingKey!));
  }

  private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

  private static void AddPasswordEncrypt(IServiceCollection services)
  {
    services.AddScoped<IPasswordEncrypt, Sha512PasswordEncrypt>();
  }
}