using Flashy.Data.Models;
using Flashy.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Flashy.Data.Configurations;

public class DeckEntityTypeConfiguration : IEntityTypeConfiguration<Deck>
{
  private static string CREATED_BY_ID = "CreatedById";

  public void Configure(EntityTypeBuilder<Deck> builder)
  {
    builder
      .ToTable("Decks");

    builder
      .Property(p => p.Id)
      .HasColumnName(nameof(Deck.Id))
      .HasColumnType("uniqueidentifier")
      .ValueGeneratedOnAdd()
      .HasValueGenerator<GuidValueGenerator>()
      .IsRequired();

    builder
      .Property(p => p.Name)
      .HasColumnName(nameof(Deck.Name))
      .HasColumnType("nvarchar(64)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.NormalizedName)
      .HasColumnName(nameof(Deck.NormalizedName))
      .HasColumnType("nvarchar(64)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.Description)
      .HasColumnName(nameof(Deck.Description))
      .HasColumnType("nvarchar(128)")
      .IsUnicode();

    builder
      .Property<Guid>(CREATED_BY_ID)
      .HasColumnName(CREATED_BY_ID)
      .HasColumnType("uniqueidentifier");

    builder
      .Property(p => p.CreatedAt)
      .HasColumnName(nameof(Deck.CreatedAt))
      .HasColumnType("datetime2")
      .IsRequired();

    builder
      .HasKey(k => k.Id)
      .HasName("PK_Decks");

    builder
      .HasIndex(i => i.NormalizedName)
      .HasDatabaseName($"IX_Decks_{nameof(Deck.NormalizedName)}")
      .IsUnique();

    builder
      .HasIndex(CREATED_BY_ID)
      .HasDatabaseName($"IX_Decks_{CREATED_BY_ID}");

    builder
      .HasIndex(i => i.CreatedAt)
      .HasDatabaseName($"IX_Decks_{nameof(Deck.CreatedAt)}");

    builder
      .HasOne(r => r.CreatedBy)
      .WithMany(r => r.CreatedDecks)
      .HasForeignKey(CREATED_BY_ID)
      .HasConstraintName("FK_Decks_CreatedBy")
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired();

    builder
      .HasMany(r => r.FlashCards)
      .WithMany()
      .UsingEntity<DeckFlashCard>();
  }
}