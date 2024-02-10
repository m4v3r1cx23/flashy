// Copyright by Tomasz Mróz under MIT license

namespace Flashy.Shared.Models;

public class Deck
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string NormalizedName { get; set; } = null!;
  public string? Description { get; set; }
  public virtual List<FlashCard> FlashCards { get; } = [];
  public virtual User CreatedBy { get; private set; } = null!;
  public DateTime CreatedAt { get; private set; }
}