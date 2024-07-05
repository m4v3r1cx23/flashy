namespace Flashy.API.Domain.Models.FlashCards;

public class Deck
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string Category { get; set; } = null!;
  public string Description { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public virtual User? CreatedBy { get; set; } = null!;
  public virtual List<FlashCard> FlashCards { get; set; } = [];
}
