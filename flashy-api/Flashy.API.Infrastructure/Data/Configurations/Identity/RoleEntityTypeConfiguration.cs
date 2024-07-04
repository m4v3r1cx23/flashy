using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Flashy.Data.Configurations.Identity;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
  public void Configure(EntityTypeBuilder<Role> builder)
  {
    builder
        .ToTable("Roles");

    builder
        .Property(p => p.Id)
        .HasColumnName(nameof(Role.Id))
        .HasColumnType("uniqueidentifier")
        .ValueGeneratedOnAdd()
        .HasValueGenerator<GuidValueGenerator>()
        .IsRequired();

    builder
        .Property(p => p.Name)
        .HasColumnName(nameof(Role.Name))
        .HasColumnType("nvarchar(256)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.NormalizedName)
        .HasColumnName(nameof(Role.NormalizedName))
        .HasColumnType("varchar(256)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.ConcurrencyStamp)
        .HasColumnName(nameof(Role.ConcurrencyStamp))
        .HasColumnType("varchar(256)")
        .IsConcurrencyToken()
        .IsRequired();

    builder
        .HasKey(k => k.Id)
        .HasName("PK_Roles");

    builder
        .HasIndex(i => i.Name)
        .HasDatabaseName($"IX_Roles_{nameof(Role.Name)}")
        .IsUnique();

    builder
        .HasIndex(i => i.NormalizedName)
        .HasDatabaseName($"IX_Roles_{nameof(Role.NormalizedName)}")
        .IsUnique();
  }
}
