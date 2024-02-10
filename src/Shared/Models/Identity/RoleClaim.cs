using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Models.Identity;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public virtual Role Role { get; set; } = null!;
}