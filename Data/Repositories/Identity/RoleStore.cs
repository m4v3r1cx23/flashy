using System.Security.Claims;
using Flashy.Data.Context;
using Flashy.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Data.Repositories.Identity;

public class RoleStore : IRoleStore<Role>, IRoleClaimStore<Role>
{
  protected ApplicationDbContext _context;

  public RoleStore(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
  {
    try
    {
      _context.Add(role);

      var result = await _context.SaveChangesAsync(cancellationToken);

      return result > 0
        ? IdentityResult.Success
        : IdentityResult.Failed(new IdentityError { Description = "Failed to create role." });
    }
    catch (Exception e)
    {
      return IdentityResult.Failed(new IdentityError { Description = e.Message });
    }
  }

  public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
  {
    try
    {
      _context.Remove(role);

      var result = _context.SaveChangesAsync(cancellationToken);

      return result.Result > 0
        ? Task.FromResult(IdentityResult.Success)
        : Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Failed to delete role." }));
    }
    catch (Exception e)
    {
      return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = e.Message }));
    }
  }

  public void Dispose()
  {
    _context.Dispose();
  }

  public Task<Role?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
  {
    try
    {
      var id = Guid.Parse(roleId);

      return _context.Roles.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
    catch (Exception)
    {
      return Task.FromResult<Role?>(null);
    }
  }

  public Task<Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
  {
    try
    {
      return _context.Roles.FirstOrDefaultAsync(u => u.NormalizedName == normalizedRoleName, cancellationToken);
    }
    catch (Exception)
    {
      return Task.FromResult<Role?>(null);
    }
  }

  public Task<string?> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
  {
    return Task.FromResult(role.NormalizedName);
  }

  public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
  {
    return Task.FromResult(role.Id.ToString());
  }

  public Task<string?> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
  {
    return Task.FromResult(role.Name);
  }

  public Task SetNormalizedRoleNameAsync(Role role, string? normalizedName, CancellationToken cancellationToken)
  {
    return Task.FromResult(role.NormalizedName = normalizedName);
  }

  public Task SetRoleNameAsync(Role role, string? roleName, CancellationToken cancellationToken)
  {
    return Task.FromResult(role.Name = roleName);
  }

  public Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
  {
    return Task.FromResult(IdentityResult.Success);
  }

  public Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
  {
    try
    {
      var roleClaim = new RoleClaim
      {
        RoleId = role.Id,
        ClaimType = claim.Type,
        ClaimValue = claim.Value
      };

      _context.RoleClaims.Add(roleClaim);

      return _context.SaveChangesAsync(cancellationToken);
    }
    catch (DbUpdateException e)
    {
      throw new Exception("Failed to add claim to role", e);
    }
  }

  public Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default)
  {
    try
    {
      var claims = _context.RoleClaims.Select(x => new Claim(x.ClaimType!, x.ClaimValue!))
                                      .ToList();

      return Task.FromResult<IList<Claim>>(claims);
    }
    catch (Exception e)
    {
      return Task.FromException<IList<Claim>>(e);
    }
  }

  public Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
  {
    try
    {
      var roleClaim = _context.RoleClaims.FirstOrDefault(x => x.RoleId == role.Id &&
                                                              x.ClaimType == claim.Type &&
                                                              x.ClaimValue == claim.Value)
        ?? throw new InvalidOperationException("Role does not have claim.");

      _context.RoleClaims.Remove(roleClaim);

      return _context.SaveChangesAsync(cancellationToken);
    }
    catch (DbUpdateException e)
    {
      throw new Exception("Failed to remove claim from role", e);
    }
  }
}