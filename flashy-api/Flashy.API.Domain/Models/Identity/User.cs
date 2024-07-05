using Flashy.API.Domain.Models.FlashCards;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>
{
  public string FirstName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string FullName => $"{FirstName} {LastName}";
  public virtual List<UserClaim> Claims { get; } = [];
  public virtual List<UserLogin> Logins { get; } = [];
  public virtual List<UserToken> Tokens { get; } = [];
  public virtual List<Role> Roles { get; } = [];
  public virtual List<Trial> Trials { get; } = [];
}
