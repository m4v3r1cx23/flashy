namespace Flashy.API.Domain.Models.FlashCards;

public class FlashCardDeck
{
  public Guid FlashCardId { get; set; }
  public Guid DeckId { get; set; }
  public virtual FlashCard FlashCard { get; set; } = null!;
  public virtual Deck Deck { get; set; } = null!;
}
