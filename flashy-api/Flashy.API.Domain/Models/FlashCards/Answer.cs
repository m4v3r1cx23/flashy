namespace Flashy.API.Domain.Models.FlashCards;

public class Answer
{
  public Guid Id { get; set; }
  public virtual Trial Trial { get; set; } = null!;
  public virtual FlashCard FlashCard { get; set; } = null!;
  public bool IsCorrect { get; set; }
}
