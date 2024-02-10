using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Models.Identity;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; private set; } = null!;
    public virtual Role Role { get; private set; } = null!;
}