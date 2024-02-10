using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Models.Identity;

public class UserClaim : IdentityUserClaim<Guid>
{
  public virtual User User { get; private set; } = null!;
}