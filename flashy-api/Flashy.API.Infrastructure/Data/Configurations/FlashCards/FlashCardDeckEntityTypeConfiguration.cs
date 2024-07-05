using Flashy.API.Domain.Models.FlashCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.API.Infrastructure.Data.Configurations.FlashCards;

public class FlashCardDeckEntityTypeConfiguration : IEntityTypeConfiguration<FlashCardDeck>
{
  public void Configure(EntityTypeBuilder<FlashCardDeck> builder)
  {
    builder
      .ToTable("FlashCardDeck");

    builder
      .Property(p => p.FlashCardId)
      .HasColumnName(nameof(FlashCardDeck.FlashCardId))
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .Property(p => p.DeckId)
      .HasColumnName(nameof(FlashCardDeck.DeckId))
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .HasKey(k => new { k.FlashCardId, k.DeckId })
      .HasName($"PK_FlashCardDeck");

    builder
      .HasOne(r => r.FlashCard)
      .WithMany()
      .HasForeignKey(f => f.FlashCardId)
      .HasConstraintName($"FX_{nameof(FlashCardDeck)}_{nameof(FlashCard)}_{nameof(FlashCard.Id)}")
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired();

    builder
      .HasOne(r => r.Deck)
      .WithMany()
      .HasForeignKey(f => f.DeckId)
      .HasForeignKey($"FX_{nameof(FlashCardDeck)}_{nameof(Deck)}_{nameof(Deck.Id)}")
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired();
  }
}
