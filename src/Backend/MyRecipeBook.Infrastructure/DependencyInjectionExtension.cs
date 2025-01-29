using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Domain.ServiceBus;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Encrypt;
using MyRecipeBook.Infrastructure.Security.Token.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Token.Access.Validator;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.OpenAI;
using MyRecipeBook.Infrastructure.Services.ServiceBus;
using MyRecipeBook.Infrastructure.Services.Storage;
using OpenAI.Chat;
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
    AddOpenAI(services, configuration);
    AddAzureStorage(services, configuration);
    AddQueue(services, configuration);

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
    services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
    services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();
    services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
    services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
    services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
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

  private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IRecipeGenerateAI, ChatGPTService>();

    var key = configuration.GetValue<string>("Settings:OpenAI:ApiKey");

    services.AddScoped(opt => new ChatClient(MyRecipeBookRuleConstants.CHAT_MODEL_VERSION, key));
  }

  private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

    if (connectionString.NotEmpty())
    {
      services.AddScoped<IBlobStorageService>(_ => new AzureStorageService(new BlobServiceClient(connectionString)));
    }
  }

  private static void AddQueue(IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
      return;
    }

    var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
    {
      TransportType = ServiceBusTransportType.AmqpWebSockets
    });

    var deleteQueue = new DeleteUserQueue(client.CreateSender("user"));
    var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("user", new ServiceBusProcessorOptions
    {
      MaxConcurrentCalls = 1
    }));

    services.AddSingleton(deleteUserProcessor);
    services.AddScoped<IDeleteUserQueue>(_ => deleteQueue);
  }
}