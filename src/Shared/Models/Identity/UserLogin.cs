using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Models.Identity;

public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User User { get; set; } = null!;
    public string NormalizedProviderDisplayName { get; set; } = null!;
}