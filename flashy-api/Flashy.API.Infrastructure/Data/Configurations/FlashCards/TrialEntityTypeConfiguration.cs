using Flashy.API.Domain.Models.FlashCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.API.Infrastructure.Data.Configurations.FlashCards;

public class TrialEntityTypeConfiguration : IEntityTypeConfiguration<Trial>
{
  public void Configure(EntityTypeBuilder<Trial> builder)
  {
    builder
      .ToTable("Trials");

    builder
      .Property(p => p.Id)
      .HasColumnName(nameof(Trial.Id))
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .Property("UserId")
      .HasColumnName("UserId")
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .Property(p => p.LastFlashCardIndex)
      .HasColumnName(nameof(Trial.LastFlashCardIndex))
      .HasColumnType("int")
      .IsRequired();

    builder
      .Property(p => p.StartedAt)
      .HasColumnName(nameof(Trial.StartedAt))
      .HasColumnType("datetime2")
      .IsRequired();

    builder
      .Property(p => p.CompletedAt)
      .HasColumnName(nameof(Trial.CompletedAt))
      .HasColumnType("datetime2");

    builder
      .HasKey(k => k.Id)
      .HasName($"PK_{nameof(Trial)}");

    builder
      .HasIndex("UserId")
      .HasDatabaseName($"IX_{nameof(Trial)}_UserId");

    builder
      .HasOne(r => r.User)
      .WithMany(r => r.Trials)
      .HasForeignKey("UserId")
      .HasConstraintName($"FX_{nameof(Trial)}_{nameof(User)}_{nameof(User.Id)}")
      .OnDelete(DeleteBehavior.Cascade);

    builder
      .HasMany(r => r.FlashCards)
      .WithMany()
      .UsingEntity<FlashCardTrial>();

    builder
      .HasIndex(i => i.StartedAt)
      .HasDatabaseName($"IX_{nameof(Trial)}_{nameof(Trial.StartedAt)}");
  }
}
