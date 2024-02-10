using Flashy.Data.Models;
using Flashy.Shared.Models;
using Flashy.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Data.Context;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
  { }

  // Tables
  public DbSet<FlashCard> Flashcards { get; set; }
  public DbSet<Deck> Decks { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // General Configuration
    modelBuilder.HasDefaultSchema("flashy");
    modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

    // Apply Model Configurations
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
  }
}
