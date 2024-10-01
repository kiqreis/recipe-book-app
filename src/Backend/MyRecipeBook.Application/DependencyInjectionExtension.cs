using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.SecurityConfig;
using MyRecipeBook.Application.UseCases.UserManagement.Create;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
  public static void AddApplication(this IServiceCollection services)
  {
    AddAutoMapper(services);
    AddPasswordEncrypt(services);
    AddUseCases(services);
  }

  private static void AddAutoMapper(IServiceCollection services)
  {
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
  }

  private static void AddUseCases(IServiceCollection services)
  {
    services.AddScoped<ICreateUser, CreateUser>();
  }

  private static void AddPasswordEncrypt(IServiceCollection services)
  {
    services.AddScoped<PasswordEncrypt>();
  }
}
