using Flashy.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.Data.Configurations.M2M;

public class UserDeckEntityTypeConfiguration : IEntityTypeConfiguration<UserDeck>
{
  private static string USER_ID = "UserId";
  private static string DECK_ID = "DeckId";

  public void Configure(EntityTypeBuilder<UserDeck> builder)
  {
    builder
       .ToTable("UsersDecks");

    builder
      .Property<Guid>(USER_ID)
      .HasColumnName(USER_ID)
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .Property<Guid>(DECK_ID)
      .HasColumnName(DECK_ID)
      .HasColumnType("uniqueidentifier")
      .IsRequired();

    builder
      .HasKey(USER_ID, DECK_ID)
      .HasName("PK_UsersDecks");

    builder
      .HasOne(r => r.User)
      .WithMany()
      .HasForeignKey(USER_ID)
      .HasConstraintName("FK_UsersDecks_Users")
      .OnDelete(DeleteBehavior.ClientNoAction)
      .IsRequired();

    builder
      .HasOne(r => r.Deck)
      .WithMany()
      .HasForeignKey(DECK_ID)
      .HasConstraintName("FK_UsersDecks_Decks")
      .OnDelete(DeleteBehavior.ClientNoAction)
      .IsRequired();
  }
}