using Microsoft.AspNetCore.Identity;

public class RoleClaim : IdentityRoleClaim<Guid>
{
  public virtual Role Role { get; set; } = null!;
}
