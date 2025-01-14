using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(3, "Add column on recipe table to save or update images")]
public class V3_AddColumnOnRecipeTable : VersionBase
{
  public override void Up()
  {
    Alter.Table("Recipes").AddColumn("ImageId").AsString().Nullable();
  }
}
