using CommonTestsUtilities.Entities;
using CommonTestsUtilities.IdEncrypt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  private User _user = default!;
  private Recipe _recipe = default!;
  private string _password = string.Empty;

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Test")
      .ConfigureServices(service =>
      {
        var descriptor = service.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

        if (descriptor is not null)
        {
          service.Remove(descriptor);
        }

        var provider = service.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        service.AddDbContext<AppDbContext>(opt =>
        {
          opt.UseInMemoryDatabase("InMemoryTesting");
          opt.UseInternalServiceProvider(provider);
        });

        var scope = service.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.EnsureDeleted();

        InitDb(dbContext);
      });
  }

  public Guid GetUserId() => _user.UserId;
  public string GetName() => _user.Name;
  public string GetEmail() => _user.Email;
  public string GetPassword() => _password;

  public string GetRecipeId() => IdEncryptBuilder.Build().Encode(_recipe.Id);
  public string GetRecipeTitle() => _recipe.Title;
  public Difficulty GetRecipeDifficulty() => _recipe.Difficulty!.Value;
  public CookingTime GetRecipeCookingTime() => _recipe.CookingTime!.Value;
  public IList<MyRecipeBook.Domain.Enums.DishType> GetDishTypes() => _recipe.DishTypes.Select(d => d.Type).ToList();

  private void InitDb(AppDbContext context)
  {
    (_user, _password) = UserBuilder.Build();
    _recipe = RecipeBuilder.Build(_user);

    context.Users.Add(_user);
    context.Recipes.Add(_recipe);

    context.SaveChanges();
  }
}
