using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Models.Identity;

public class Role : IdentityRole<Guid>
{
    public virtual List<User> Users { get; set; } = [];
    public virtual List<RoleClaim> Claims { get; set; } = [];
}