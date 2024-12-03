using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Application.UseCases.UserManagement.Login;
using MyRecipeBook.Application.UseCases.UserManagement.Profile;
using MyRecipeBook.Application.UseCases.UserManagement.Update;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
  public static void AddApplication(this IServiceCollection services)
  {
    AddAutoMapper(services);
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
    services.AddScoped<IUpdateUser, UpdateUser>();
    services.AddScoped<IChangePassword, ChangePassword>();
  }
}
