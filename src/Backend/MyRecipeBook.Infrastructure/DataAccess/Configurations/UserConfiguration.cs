using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.ToTable("Users");

    builder.Property(u => u.Id)
      .HasColumnName("Id")
      .HasColumnType("int")
      .UseIdentityColumn();

    builder.Property(u => u.IsActive)
      .HasColumnName("IsActive")
      .HasColumnType("bit")
      .HasDefaultValue(true);

    builder.Property(u => u.CreateAt)
      .HasColumnName("CreateAt")
      .HasColumnType("datetime")
      .HasDefaultValueSql("getdate()");

    builder.Property(u => u.Name)
      .HasColumnName("Name")
      .HasColumnType("nvarchar")
      .HasMaxLength(120)
      .IsRequired();

    builder.Property(u => u.Email)
      .HasColumnName("Email")
      .HasColumnType("varchar")
      .HasMaxLength(160)
      .IsRequired();

    builder.Property(u => u.Password)
      .HasColumnName("Password")
      .HasColumnType("nvarchar")
      .HasMaxLength(64)
      .IsRequired();
  }
}
