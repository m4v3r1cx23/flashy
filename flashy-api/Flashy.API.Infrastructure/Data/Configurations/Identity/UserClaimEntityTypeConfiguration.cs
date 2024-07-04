using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.Data.Configurations.Identity;

public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<UserClaim>
{
  public void Configure(EntityTypeBuilder<UserClaim> builder)
  {
    builder
        .ToTable("UserClaims");

    builder
        .Property(p => p.Id)
        .HasColumnName(nameof(UserClaim.Id))
        .HasColumnType("int")
        .ValueGeneratedOnAdd()
        .UseIdentityColumn()
        .IsRequired();

    builder
        .Property(p => p.UserId)
        .HasColumnName(nameof(UserClaim.UserId))
        .HasColumnType("uniqueidentifier")
        .IsRequired();

    builder
        .Property(p => p.ClaimType)
        .HasColumnName(nameof(UserClaim.ClaimType))
        .HasColumnType("varchar(128)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.ClaimValue)
        .HasColumnName(nameof(UserClaim.ClaimValue))
        .HasColumnType("varchar(128)")
        .IsRequired()
        .IsUnicode();

    builder
        .HasKey(k => k.Id)
        .HasName("PK_UserClaims");

    builder
        .HasIndex(i => i.UserId)
        .HasDatabaseName($"IX_UserClaims_{nameof(UserClaim.UserId)}");

    builder
        .HasOne(r => r.User)
        .WithMany(r => r.Claims)
        .HasForeignKey(f => f.UserId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();
  }
}
