using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Flashy.Data.Configurations.Identity;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder
        .ToTable("Users");

    builder
        .Property(p => p.Id)
        .HasColumnName(nameof(User.Id))
        .HasColumnType("uniqueidentifier")
        .ValueGeneratedOnAdd()
        .HasValueGenerator<GuidValueGenerator>()
        .IsRequired();

    builder
        .Property(p => p.FirstName)
        .HasColumnName(nameof(User.FirstName))
        .HasColumnType("nvarchar(64)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.LastName)
        .HasColumnName(nameof(User.LastName))
        .HasColumnType("nvarchar(64)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.UserName)
        .HasColumnName(nameof(User.UserName))
        .HasColumnType("nvarchar(256)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.NormalizedUserName)
        .HasColumnName(nameof(User.NormalizedUserName))
        .HasColumnType("nvarchar(256)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.Email)
        .HasColumnName(nameof(User.Email))
        .HasColumnType("nvarchar(320)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.NormalizedEmail)
        .HasColumnName(nameof(User.NormalizedEmail))
        .HasColumnType("nvarchar(320)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.EmailConfirmed)
        .HasColumnName(nameof(User.EmailConfirmed))
        .HasColumnType("bit")
        .HasDefaultValue(true) // TODO: Change to false after Development is complete
        .IsRequired();

    builder
        .Property(p => p.PhoneNumber)
        .HasColumnName(nameof(User.PhoneNumber))
        .HasColumnType("nvarchar(24)");

    builder
        .Property(p => p.PhoneNumberConfirmed)
        .HasColumnName(nameof(User.PhoneNumberConfirmed))
        .HasColumnType("bit")
        .HasDefaultValue(false)
        .IsRequired();

    builder
        .Property(p => p.TwoFactorEnabled)
        .HasColumnName(nameof(User.TwoFactorEnabled))
        .HasColumnType("bit")
        .HasDefaultValue(false)
        .IsRequired();

    builder
        .Property(p => p.SecurityStamp)
        .HasColumnName(nameof(User.SecurityStamp))
        .HasColumnType("nvarchar(256)")
        .IsRequired();

    builder
        .Property(p => p.PasswordHash)
        .HasColumnName(nameof(User.PasswordHash))
        .HasColumnType("nvarchar(256)")
        .IsRequired();

    builder
        .Property(p => p.ConcurrencyStamp)
        .HasColumnName(nameof(User.ConcurrencyStamp))
        .HasColumnType("nvarchar(256)")
        .IsConcurrencyToken()
        .IsRequired();

    builder
        .Property(p => p.LockoutEnabled)
        .HasColumnName(nameof(User.LockoutEnabled))
        .HasColumnType("bit")
        .HasDefaultValue(true)
        .IsRequired();

    builder
        .Property(p => p.AccessFailedCount)
        .HasColumnName(nameof(User.AccessFailedCount))
        .HasColumnType("int")
        .HasDefaultValue(0)
        .IsRequired();

    builder
        .Property(p => p.LockoutEnd)
        .HasColumnName(nameof(User.LockoutEnd))
        .HasColumnType("datetimeoffset");

    builder
        .HasKey(k => k.Id)
        .HasName("PK_Users");

    builder
        .HasIndex(i => i.UserName)
        .HasDatabaseName($"IX_Users_{nameof(User.UserName)}")
        .IsUnique();

    builder
        .HasIndex(i => i.NormalizedUserName)
        .HasDatabaseName($"IX_Users_{nameof(User.NormalizedUserName)}")
        .IsUnique();

    builder
        .HasIndex(i => i.Email)
        .HasDatabaseName($"IX_Users_{nameof(User.Email)}")
        .IsUnique();

    builder
        .HasIndex(i => i.NormalizedEmail)
        .HasDatabaseName($"IX_Users_{nameof(User.NormalizedEmail)}")
        .IsUnique();

    builder
        .HasMany(r => r.Roles)
        .WithMany(r => r.Users)
        .UsingEntity<UserRole>();
  }
}
