using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flashy.API.Infrastructure.Data;

public class FlashyAPIDBContext(DbContextOptions<FlashyAPIDBContext> options) :
  IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // General Configuration
    modelBuilder.HasDefaultSchema("flashy");
    modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

    // Apply Model Configurations
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlashyAPIDBContext).Assembly);
  }
}
