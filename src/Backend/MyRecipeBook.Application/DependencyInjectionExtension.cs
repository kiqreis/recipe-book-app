using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.SecurityConfig;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Application.UseCases.UserManagement.Login;
using MyRecipeBook.Application.UseCases.UserManagement.Profile;

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
    services.AddScoped<ILogin, Login>();
    services.AddScoped<IGetUserProfile, GetUserProfile>();
  }

  private static void AddPasswordEncrypt(IServiceCollection services)
  {
    services.AddScoped<PasswordEncrypt>();
  }
}
