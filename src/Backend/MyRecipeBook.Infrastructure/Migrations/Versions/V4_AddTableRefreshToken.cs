using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(4, "Create table to save refresh tokens")]
public class V4_AddTableRefreshToken : VersionBase
{
  public override void Up()
  {
    CreateTable("RefreshTokens")
      .WithColumn("Value").AsString().NotNullable()
      .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_RefreshTokens_User_Id", "Users", "Id");
  }
}
