using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.Data.Configurations.Identity;

public class UserLoginEntityTypeConfiguration : IEntityTypeConfiguration<UserLogin>
{
  public void Configure(EntityTypeBuilder<UserLogin> builder)
  {
    builder
        .ToTable("UserLogins");

    builder
        .Property(p => p.UserId)
        .HasColumnName(nameof(UserLogin.UserId))
        .HasColumnType("uniqueidentifier")
        .ValueGeneratedNever()
        .IsRequired();

    builder
        .Property(p => p.LoginProvider)
        .HasColumnName(nameof(UserLogin.LoginProvider))
        .HasColumnType("varchar(128)")
        .IsRequired();

    builder
        .Property(p => p.ProviderKey)
        .HasColumnName(nameof(UserLogin.ProviderKey))
        .HasColumnType("varchar(128)")
        .IsRequired();

    builder
        .Property(p => p.ProviderDisplayName)
        .HasColumnName(nameof(UserLogin.ProviderDisplayName))
        .HasColumnType("nvarchar(128)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.NormalizedProviderDisplayName)
        .HasColumnName(nameof(UserLogin.NormalizedProviderDisplayName))
        .HasColumnType("varchar(128)")
        .IsRequired()
        .IsUnicode();

    builder
        .HasKey(k => new { k.LoginProvider, k.ProviderKey })
        .HasName("PK_UserLogins");

    builder
        .HasIndex(i => i.UserId)
        .HasDatabaseName($"IX_UserLogins_{nameof(UserLogin.UserId)}");

    builder
        .HasIndex(i => i.ProviderDisplayName)
        .HasDatabaseName($"IX_UserLogins_{nameof(UserLogin.ProviderDisplayName)}");

    builder
        .HasIndex(i => i.NormalizedProviderDisplayName)
        .HasDatabaseName($"IX_UserLogins_{nameof(UserLogin.NormalizedProviderDisplayName)}");

    builder
        .HasOne(r => r.User)
        .WithMany(r => r.Logins)
        .HasForeignKey(f => f.UserId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();
  }
}
