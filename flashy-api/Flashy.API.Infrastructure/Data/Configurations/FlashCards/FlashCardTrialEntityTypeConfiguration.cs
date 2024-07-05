using Flashy.API.Domain.Models.FlashCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.API.Infrastructure.Data.Configurations.FlashCards;

public class FlashCardTrialEntityTypeConfiguration : IEntityTypeConfiguration<FlashCardTrial>
{
  public void Configure(EntityTypeBuilder<FlashCardTrial> builder)
  {
    builder
      .ToTable("FlashCardTrial");

    builder
      .Property(p => p.FlashCardId)
      .HasColumnName(nameof(FlashCardTrial.FlashCardId))
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .Property(p => p.TrialId)
      .HasColumnName(nameof(FlashCardTrial.TrialId))
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .HasKey(k => new { k.FlashCardId, k.TrialId })
      .HasName($"PK_FlashCardTrial");

    builder
      .HasOne(r => r.FlashCard)
      .WithMany()
      .HasForeignKey(f => f.FlashCardId)
      .HasConstraintName($"FX_{nameof(FlashCardTrial)}_{nameof(FlashCard)}_{nameof(FlashCard.Id)}")
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired();

    builder
      .HasOne(r => r.Trial)
      .WithMany()
      .HasForeignKey(f => f.TrialId)
      .HasConstraintName($"FX_{nameof(FlashCardTrial)}_{nameof(Trial)}_{nameof(Trial.Id)}")
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired();
  }
}
