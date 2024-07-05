namespace Flashy.API.Domain.Models.FlashCards;

public class FlashCard
{
  public Guid Id { get; set; }
  public string Question { get; set; } = null!;
  public string Answer { get; set; } = null!;
  public string Hint { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public virtual User? CreatedBy { get; set; } = null!;
  public string Hash { get; set; } = null!;
}
