using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace MyRecipeBook.Infrastructure.Migrations;

public class DbMigration
{
  public static void Migration(string connection, IServiceProvider provider)
  {
    EnsureDatabaseCreated(connection);
    MigrationDb(provider);
  }

  private static void EnsureDatabaseCreated(string connection)
  {
    var connectionStringBuilder = new SqlConnectionStringBuilder(connection);
    var dbName = connectionStringBuilder.InitialCatalog;

    connectionStringBuilder.Remove("Database");

    using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

    var parameters = new DynamicParameters();
    parameters.Add("name", dbName);

    var records = dbConnection.Query("SELECT * FROM sys.databases WHERE name = @name", parameters);

    if (records.Any() == false)
    {
      dbConnection.Execute($"CREATE DATABASE {dbName}");
    }
  }

  private static void MigrationDb(IServiceProvider provider)
  {
    var runner = provider.GetRequiredService<IMigrationRunner>();

    runner.ListMigrations();
    runner.MigrateUp();
  }
}
