using Flashy.API.Domain.Models.FlashCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.API.Infrastructure.Data.Configurations.FlashCards;

public class DeckCardEntityTypeConfiguration : IEntityTypeConfiguration<Deck>
{
  public void Configure(EntityTypeBuilder<Deck> builder)
  {
    builder
      .ToTable("Decks");

    builder
      .Property(p => p.Id)
      .HasColumnName(nameof(Deck.Id))
      .HasColumnType("uniqueidentifier")
      .ValueGeneratedOnAdd()
      .IsRequired();

    builder
      .Property(p => p.Name)
      .HasColumnName(nameof(Deck.Name))
      .HasColumnType("nvarchar(64)")
      .IsUnicode()
      .IsRequired();

    builder
      .Property(p => p.Category)
      .HasColumnName(nameof(Deck.Category))
      .HasColumnType("nvarchar(32)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.Description)
      .HasColumnName(nameof(Deck.Description))
      .HasColumnType("nvarchar(256)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.CreatedAt)
      .HasColumnName(nameof(Deck.CreatedAt))
      .HasColumnType("datetime2")
      .IsRequired();

    builder
      .Property(p => p.UpdatedAt)
      .HasColumnName(nameof(Deck.UpdatedAt))
      .HasColumnType("datetime2")
      .IsRequired();

    builder
      .Property("CreatedById")
      .HasColumnName("CreatedById")
      .HasColumnType("uniqueidentifier");

    builder
      .HasOne(r => r.CreatedBy)
      .WithMany()
      .HasForeignKey("CreatedById")
      .HasConstraintName($"FX_{nameof(Deck)}_{nameof(User)}_{nameof(User.Id)}")
      .OnDelete(DeleteBehavior.SetNull);

    builder
      .HasKey(k => k.Id)
      .HasName($"PK_{nameof(Deck)}");

    builder
      .HasIndex(i => i.Name)
      .HasDatabaseName($"IX_{nameof(Deck)}_{nameof(Deck.Name)}");

    builder
      .HasIndex(i => i.Category)
      .HasDatabaseName($"IX_{nameof(Deck)}_{nameof(Deck.Category)}");

    builder
      .HasIndex(i => i.CreatedAt)
      .HasDatabaseName($"IX_{nameof(Deck)}_{nameof(Deck.CreatedAt)}");

    builder
      .HasIndex(i => i.UpdatedAt)
      .HasDatabaseName($"IX_{nameof(Deck)}_{nameof(Deck.UpdatedAt)}");

    builder
      .HasOne(r => r.CreatedBy)
      .WithMany()
      .HasForeignKey("CreatedById")
      .HasConstraintName($"FX_{nameof(Deck)}_{nameof(User)}_{nameof(User.Id)}")
      .OnDelete(DeleteBehavior.SetNull);

    builder
      .HasMany(r => r.FlashCards)
      .WithMany()
      .UsingEntity<FlashCardDeck>();
  }
}
