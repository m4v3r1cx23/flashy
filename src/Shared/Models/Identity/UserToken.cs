using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Models.Identity;

public class UserToken : IdentityUserToken<Guid>
{
    public virtual User User { get; set; } = null!;
}