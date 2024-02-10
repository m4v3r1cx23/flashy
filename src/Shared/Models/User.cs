// Copyright by Tomasz Mróz under MIT license

using Flashy.Shared.Models.Identity;

using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Models;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
    public virtual List<FlashCard> CreatedFlashCards { get; } = [];
    public virtual List<Deck> CreatedDecks { get; } = [];
    public virtual List<Deck> Decks { get; } = [];
    public virtual List<UserClaim> Claims { get; } = [];
    public virtual List<UserLogin> Logins { get; } = [];
    public virtual List<UserToken> Tokens { get; } = [];
    public virtual List<Role> Roles { get; } = [];
}