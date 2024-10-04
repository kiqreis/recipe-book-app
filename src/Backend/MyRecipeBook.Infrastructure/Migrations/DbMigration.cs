using Dapper;
using Microsoft.Data.SqlClient;

namespace MyRecipeBook.Infrastructure.Migrations;

public class DbMigration
{
  public static void Migration(string connection)
  {
    EnsureDatabaseCreated(connection);
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
}
