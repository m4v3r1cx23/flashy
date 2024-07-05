using Flashy.API.Models.Answer;

namespace Flashy.API.Models.Trial;

public class TrialModel
{
  public virtual List<AnswerModel> Answers { get; set; } = null!;
  public int LastFlashCardIndex { get; set; }
  public DateTime? CompletedAt { get; set; }
}
