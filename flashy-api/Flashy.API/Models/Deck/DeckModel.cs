namespace Flashy.API.Models.Deck;

public class DeckModel
{
  public string Name { get; set; } = null!;
  public string Category { get; set; } = null!;
  public string Description { get; set; } = null!;
  public List<Guid> FlashCardIds { get; set; } = [];
}
