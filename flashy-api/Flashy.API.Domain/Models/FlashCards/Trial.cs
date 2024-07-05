namespace Flashy.API.Domain.Models.FlashCards;

public class Trial
{
  public Guid Id { get; set; }
  public virtual User User { get; set; } = null!;
  public virtual List<FlashCard> FlashCards { get; set; } = null!;
  public virtual List<Answer> Answers { get; set; } = null!;
  public int LastFlashCardIndex { get; set; }
  public DateTime StartedAt { get; set; }
  public DateTime? CompletedAt { get; set; }
}
