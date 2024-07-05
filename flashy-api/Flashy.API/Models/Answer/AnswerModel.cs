namespace Flashy.API.Models.Answer;

public class AnswerModel
{
  public Guid Id { get; set; }
  public Guid FlashCardId { get; set; }
  public bool IsCorrect { get; set; }
}
