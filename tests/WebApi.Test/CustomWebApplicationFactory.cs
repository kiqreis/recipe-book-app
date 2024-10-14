using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
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
      }); 
  }
}
