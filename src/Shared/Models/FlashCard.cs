// Copyright by Tomasz Mróz under MIT license

namespace Flashy.Shared.Models;

public class FlashCard
{
  public Guid Id { get; set; }
  public string Front { get; set; } = null!;
  public string NormalizedFront { get; set; } = null!;
  public string Back { get; set; } = null!;
  public string NormalizedBack { get; set; } = null!;
  public string? Hint { get; private set; }
  public virtual User CreatedBy { get; private set; } = null!;
  public DateTime CreatedAt { get; private set; }
}