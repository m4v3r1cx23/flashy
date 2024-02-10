using Flashy.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.Data.Configurations.M2M;

public class DeckFlashCardEntityTypeConfiguration : IEntityTypeConfiguration<DeckFlashCard>
{
    private static readonly string DECK_ID = "DeckId";
    private static readonly string FLASH_CARD_ID = "FlashCardId";

    public void Configure(EntityTypeBuilder<DeckFlashCard> builder)
    {
        builder
            .ToTable("DecksFlashCards");

        builder
            .Property<Guid>(DECK_ID)
            .HasColumnName(DECK_ID)
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder
            .Property<Guid>(FLASH_CARD_ID)
            .HasColumnName(FLASH_CARD_ID)
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder
            .HasKey(DECK_ID, FLASH_CARD_ID)
            .HasName("PK_DecksFlashCards");

        builder
            .HasOne(r => r.Deck)
            .WithMany()
            .HasForeignKey(DECK_ID)
            .HasConstraintName("FK_DecksFlashCards_Decks")
            .OnDelete(DeleteBehavior.ClientNoAction)
            .IsRequired();

        builder
            .HasOne(r => r.FlashCard)
            .WithMany()
            .HasForeignKey(FLASH_CARD_ID)
            .HasConstraintName("FK_DecksFlashCards_FlashCards")
            .OnDelete(DeleteBehavior.ClientNoAction)
            .IsRequired();
    }
}