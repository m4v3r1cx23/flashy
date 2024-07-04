using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.Data.Configurations.Identity;

public class RoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<RoleClaim>
{
  public void Configure(EntityTypeBuilder<RoleClaim> builder)
  {
    builder
        .ToTable("RoleClaims");

    builder
        .Property(p => p.Id)
        .HasColumnName(nameof(RoleClaim.Id))
        .HasColumnType("int")
        .ValueGeneratedOnAdd()
        .UseIdentityColumn()
        .IsRequired();

    builder
        .Property(p => p.RoleId)
        .HasColumnName(nameof(RoleClaim.RoleId))
        .HasColumnType("uniqueidentifier")
        .IsRequired();

    builder
        .Property(p => p.ClaimType)
        .HasColumnName(nameof(RoleClaim.ClaimType))
        .HasColumnType("varchar(128)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.ClaimValue)
        .HasColumnName(nameof(RoleClaim.ClaimValue))
        .HasColumnType("varchar(128)")
        .IsRequired()
        .IsUnicode();

    builder
        .HasKey(k => k.Id)
        .HasName("PK_RoleClaims");

    builder
        .HasIndex(i => i.RoleId)
        .HasDatabaseName($"IX_RoleClaims_{nameof(RoleClaim.RoleId)}");

    builder
        .HasOne(r => r.Role)
        .WithMany(r => r.Claims)
        .HasForeignKey(f => f.RoleId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();
  }
}
