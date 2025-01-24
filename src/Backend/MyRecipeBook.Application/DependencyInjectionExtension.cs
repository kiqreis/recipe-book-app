using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.Mapping;
using MyRecipeBook.Application.UseCases.RecipeManagement.Create;
using MyRecipeBook.Application.UseCases.RecipeManagement.Dashboard;
using MyRecipeBook.Application.UseCases.RecipeManagement.Delete;
using MyRecipeBook.Application.UseCases.RecipeManagement.Filter;
using MyRecipeBook.Application.UseCases.RecipeManagement.Generate;
using MyRecipeBook.Application.UseCases.RecipeManagement.GetById;
using MyRecipeBook.Application.UseCases.RecipeManagement.Image;
using MyRecipeBook.Application.UseCases.RecipeManagement.Update;
using MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Application.UseCases.UserManagement.Delete.DeleteAccount;
using MyRecipeBook.Application.UseCases.UserManagement.Delete.Request;
using MyRecipeBook.Application.UseCases.UserManagement.Login;
using MyRecipeBook.Application.UseCases.UserManagement.Profile;
using MyRecipeBook.Application.UseCases.UserManagement.Update;
using Sqids;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
  public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
  {
    AddAutoMapper(services);
    AddIdEncoder(services, configuration);
    AddUseCases(services);
  }

  private static void AddAutoMapper(IServiceCollection services)
  {
    //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddScoped(options => new MapperConfiguration(opt =>
    {
      var sqids = options.GetService<SqidsEncoder<long>>()!;

      opt.AddProfile(new MappingProfile(sqids));
    }).CreateMapper());
  }

  private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
  {
    var sqids = new SqidsEncoder<long>(new()
    {
      MinLength = 3,
      Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
    });

    services.AddSingleton(sqids);
  }

  private static void AddUseCases(IServiceCollection services)
  {
    services.AddScoped<ICreateUser, CreateUser>();
    services.AddScoped<ILogin, Login>();
    services.AddScoped<IGetUserProfile, GetUserProfile>();
    services.AddScoped<IUpdateUser, UpdateUser>();
    services.AddScoped<IChangePassword, ChangePassword>();
    services.AddScoped<ICreateRecipe, CreateRecipe>();
    services.AddScoped<IFilterRecipe, FilterRecipe>();
    services.AddScoped<IGetRecipeById, GetRecipeById>();
    services.AddScoped<IDeleteRecipe, DeleteRecipe>();
    services.AddScoped<IUpdateRecipe, UpdateRecipe>();
    services.AddScoped<IGetDashboard, GetDashboard>();
    services.AddScoped<IRecipeGenerate, RecipeGenerate>();
    services.AddScoped<IAddUpdateImage, AddUpdateImage>();
    services.AddScoped<IDeleteAccountUser, DeleteUserAccount>();
    services.AddScoped<IRequestDeleteUser, RequestDeleteUser>();
  }
}
