using Flashy.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Flashy.Data.Configurations;

public class FlashCardEntityTypeConfiguration : IEntityTypeConfiguration<FlashCard>
{
  private static string CREATED_BY_ID = "CreatedById";

  public void Configure(EntityTypeBuilder<FlashCard> builder)
  {
    builder
      .ToTable("FlashCards");

    builder
      .Property(p => p.Id)
      .HasColumnName(nameof(FlashCard.Id))
      .HasColumnType("uniqueidentifier")
      .ValueGeneratedOnAdd()
      .HasValueGenerator<GuidValueGenerator>()
      .IsRequired();

    builder
      .Property(p => p.Front)
      .HasColumnName(nameof(FlashCard.Front))
      .HasColumnType("nvarchar(64)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.NormalizedFront)
      .HasColumnName(nameof(FlashCard.NormalizedFront))
      .HasColumnType("nvarchar(64)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.Back)
      .HasColumnName(nameof(FlashCard.Back))
      .HasColumnType("nvarchar(64)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.NormalizedBack)
      .HasColumnName(nameof(FlashCard.NormalizedBack))
      .HasColumnType("nvarchar(64)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.Hint)
      .HasColumnName(nameof(FlashCard.Hint))
      .HasColumnType("nvarchar(128)")
      .IsUnicode();

    builder
      .Property<Guid>(CREATED_BY_ID)
      .HasColumnName(CREATED_BY_ID)
      .HasColumnType("uniqueidentifier");

    builder
      .Property(p => p.CreatedAt)
      .HasColumnName(nameof(FlashCard.CreatedAt))
      .HasColumnType("datetime2")
      .IsRequired();

    builder
      .HasKey(k => k.Id)
      .HasName("PK_FlashCards");

    builder
      .HasIndex(i => i.NormalizedFront)
      .HasDatabaseName($"IX_FlashCards_{nameof(FlashCard.NormalizedFront)}")
      .IsUnique();

    builder
      .HasIndex(i => i.NormalizedBack)
      .HasDatabaseName($"IX_FlashCards_{nameof(FlashCard.NormalizedBack)}")
      .IsUnique();

    builder
      .HasIndex(CREATED_BY_ID)
      .HasDatabaseName($"IX_FlashCards_CreatedBy");

    builder
      .HasIndex(i => i.CreatedAt)
      .HasDatabaseName($"IX_FlashCards_CreatedAt");

    builder
      .HasOne(r => r.CreatedBy)
      .WithMany(r => r.CreatedFlashCards)
      .HasForeignKey(CREATED_BY_ID)
      .HasConstraintName("FK_FlashCards_CreatedBy")
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired();
  }
}