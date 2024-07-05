namespace Flashy.API.Domain.Models.FlashCards;

public class FlashCardTrial
{
  public Guid FlashCardId { get; set; }
  public Guid TrialId { get; set; }
  public virtual FlashCard FlashCard { get; set; } = null!;
  public virtual Trial Trial { get; set; } = null!;
}
