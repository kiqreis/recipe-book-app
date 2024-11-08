using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(1, "Create table Users")]
public class V1_AddUserTableMigration : VersionBase
{
  public override void Up()
  {
    CreateTable("Users")
      .WithColumn("Name").AsString(120).NotNullable()
      .WithColumn("Email").AsString(160).NotNullable()
      .WithColumn("Password").AsString(128).NotNullable()
      .WithColumn("UserId").AsGuid().NotNullable();
  }
}
