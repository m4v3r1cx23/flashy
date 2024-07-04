using Microsoft.AspNetCore.Identity;

public class UserLogin : IdentityUserLogin<Guid>
{
  public virtual User User { get; set; } = null!;
  public string NormalizedProviderDisplayName { get; set; } = null!;
}
