using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.Data.Configurations.Identity;

public class UserTokenEntityTypeConfiguration : IEntityTypeConfiguration<UserToken>
{
  public void Configure(EntityTypeBuilder<UserToken> builder)
  {
    builder
        .ToTable("UserTokens");

    builder
        .Property(p => p.UserId)
        .HasColumnName(nameof(UserToken.UserId))
        .HasColumnType("uniqueidentifier")
        .ValueGeneratedNever()
        .IsRequired();

    builder
        .Property(p => p.LoginProvider)
        .HasColumnName(nameof(UserToken.LoginProvider))
        .HasColumnType("nvarchar(128)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.Name)
        .HasColumnName(nameof(UserToken.Name))
        .HasColumnType("nvarchar(128)")
        .IsRequired()
        .IsUnicode();

    builder
        .Property(p => p.Value)
        .HasColumnName(nameof(UserToken.Value))
        .HasColumnType("nvarchar(256)")
        .IsRequired();

    builder
        .HasKey(k => new { k.UserId, k.LoginProvider, k.Name })
        .HasName("PK_UserTokens");

    builder
        .HasOne(r => r.User)
        .WithMany(r => r.Tokens)
        .HasForeignKey(f => f.UserId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();
  }
}
