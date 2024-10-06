using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(1, "Create table Users")]
public class V1_AddUserTableMigration : VersionBase
{
  public override void Up()
  {
    Create.Table("Users")
      .WithColumn("Name").AsString(120).NotNullable()
      .WithColumn("Email").AsString(160).NotNullable()
      .WithColumn("Password").AsString(128).NotNullable();
  }
}
