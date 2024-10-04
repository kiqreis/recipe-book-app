using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ExtensionConfiguration
{
  public static string ConnectionString(this IConfiguration configuration)
  {
    return configuration.GetConnectionString("DefaultConnection")!;
  }
}
