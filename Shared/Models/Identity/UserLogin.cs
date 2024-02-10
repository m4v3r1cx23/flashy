using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Models.Identity;

public class UserLogin : IdentityUserLogin<Guid>
{
  public virtual User User { get; private set; } = null!;
  public string NormalizedProviderDisplayName { get; private set; } = null!;
}