using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.Data.Configurations.Identity;

public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
{
  public void Configure(EntityTypeBuilder<UserRole> builder)
  {
    builder
        .ToTable("UserRoles");

    builder
        .Property(p => p.UserId)
        .HasColumnName(nameof(UserRole.UserId))
        .HasColumnType("uniqueidentifier")
        .IsRequired();

    builder
        .Property(p => p.RoleId)
        .HasColumnName(nameof(UserRole.RoleId))
        .HasColumnType("uniqueidentifier")
        .IsRequired();

    builder
        .HasKey(k => new { k.UserId, k.RoleId })
        .HasName("PK_UserRoles");

    builder
        .HasOne(r => r.User)
        .WithMany()
        .HasForeignKey(f => f.UserId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();

    builder
        .HasOne(r => r.Role)
        .WithMany()
        .HasForeignKey(f => f.RoleId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();
  }
}
