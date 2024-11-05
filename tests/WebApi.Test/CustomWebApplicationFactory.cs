using CommonTestsUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  private User _user = default!;
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

  public string GetName() => _user.Name;
  public string GetEmail() => _user.Email;
  public string GetPassword() => _password;

  private void InitDb(AppDbContext context)
  {
    (_user, _password) = UserBuilder.Build();

    context.Users.Add(_user);
    context.SaveChanges();
  }
}
