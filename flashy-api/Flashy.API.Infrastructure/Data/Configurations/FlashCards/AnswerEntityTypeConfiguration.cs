using Flashy.API.Domain.Models.FlashCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.API.Infrastructure.Data.Configurations.FlashCards;

public class AnswerEntityTypeConfiguration : IEntityTypeConfiguration<Answer>
{
  public void Configure(EntityTypeBuilder<Answer> builder)
  {
    builder
      .ToTable("Answers");

    builder
      .Property(p => p.Id)
      .HasColumnName(nameof(Answer.Id))
      .HasColumnType("uniqueidentifier")
      .ValueGeneratedOnAdd()
      .IsRequired();

    builder
      .Property("TrialId")
      .HasColumnName("TrialId")
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .Property("FlashCardId")
      .HasColumnName("FlashCardId")
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .Property(p => p.IsCorrect)
      .HasColumnName(nameof(Answer.IsCorrect))
      .HasColumnType("bit")
      .IsRequired();

    builder
      .HasKey(k => k.Id)
      .HasName($"PK_{nameof(Answer)}");

    builder
      .HasIndex("TrialId")
      .HasDatabaseName($"IX_{nameof(Answer)}_TrialId");

    builder
      .HasIndex("FlashCardId")
      .HasDatabaseName($"IX_{nameof(Answer)}_FlashCardId");

    builder
      .HasOne(r => r.Trial)
      .WithMany(r => r.Answers)
      .HasForeignKey("TrialId")
      .HasConstraintName($"FX_{nameof(Answer)}_{nameof(Trial)}_{nameof(Trial.Id)}")
      .IsRequired();

    builder
      .HasOne(r => r.FlashCard)
      .WithMany()
      .HasForeignKey("FlashCardId")
      .HasConstraintName($"FX_{nameof(Answer)}_{nameof(FlashCard)}_{nameof(FlashCard.Id)}")
      .IsRequired();
  }
}
