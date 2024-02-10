using Flashy.Shared.Models;

namespace Flashy.Data.Models;

public class UserDeck
{
    public virtual User User { get; private set; } = null!;
    public virtual Deck Deck { get; private set; } = null!;
}