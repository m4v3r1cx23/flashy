using Flashy.Data.Context;
using Flashy.Shared.Models;
using Flashy.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Data.Repositories.Identity;

public class UserRoleStore : UserStore, IUserRoleStore<User>
{
  public UserRoleStore(ApplicationDbContext context) : base(context)
  { }

  public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
  {
    try
    {
      var role = _context.Roles.FirstOrDefault(x => x.Name == roleName)
        ?? throw new InvalidOperationException($"Role {roleName} does not exist.");

      user.Roles.Add(role);

      return Task.CompletedTask;
    }
    catch (Exception e)
    {
      return Task.FromException(e);
    }
  }

  public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
  {
    var roles = user.Roles.Select(x => x.Name)
                          .ToList();

    return Task.FromResult<IList<string>>(roles!);
  }

  public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
  {
    try
    {
      var users = await _context.Roles.Where(x => x.Name == roleName)
                                      .SelectMany(x => x.Users)
                                      .Distinct()
                                      .ToListAsync();

      return users;
    }
    catch (Exception e)
    {
      return await Task.FromException<IList<User>>(e);
    }
  }

  public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
  {
    return Task.FromResult(user.Roles.Any(x => x.Name == roleName));
  }

  public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
  {
    try
    {
      var role = _context.Roles.FirstOrDefault(x => x.Name == roleName)
        ?? throw new InvalidOperationException($"Role {roleName} does not exist.");

      user.Roles.Remove(role);

      return Task.CompletedTask;
    }
    catch (Exception e)
    {
      return Task.FromException(e);
    }
  }

  Task<User?> IUserStore<User>.FindByIdAsync(string userId, CancellationToken cancellationToken)
  {
    try
    {
      var id = Guid.Parse(userId);

      return _context.Users.FirstOrDefaultAsync(x => x.Id == id,
                                                cancellationToken: cancellationToken);
    }
    catch (Exception)
    {
      return Task.FromResult<User?>(null);
    }
  }

  Task<User?> IUserStore<User>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
  {
    try
    {
      return _context.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUserName,
                                                cancellationToken);
    }
    catch (Exception)
    {
      return Task.FromResult<User?>(null);
    }
  }
}