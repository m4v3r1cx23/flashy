using Flashy.Shared.Models;

namespace Flashy.Data.Models;

public class DeckFlashCard
{
    public virtual Deck Deck { get; private set; } = null!;
    public virtual FlashCard FlashCard { get; private set; } = null!;
}