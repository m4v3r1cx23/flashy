using System.Security.Claims;

using Flashy.Data.Context;
using Flashy.Shared.Models;
using Flashy.Shared.Models.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Data.Repositories.Identity;

public class UserStore : IUserStore<User>,
    IProtectedUserStore<User>,
    IUserAuthenticationTokenStore<User>,
    IUserAuthenticatorKeyStore<User>,
    IUserClaimStore<User>,
    IUserEmailStore<User>,
    IUserLockoutStore<User>,
    IUserTwoFactorStore<User>,
    IUserTwoFactorRecoveryCodeStore<User>,
    IUserSecurityStampStore<User>,
    IUserLoginStore<User>,
    IUserPasswordStore<User>,
    IUserPhoneNumberStore<User>
{
    protected ApplicationDbContext _context;

    public UserStore(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<string?> GetTokenAsync(User user, string loginProvider, string name,
        CancellationToken cancellationToken)
    {
        try
        {
            return _context.UserLogins.Where(u =>
                    u.UserId == user.Id && u.LoginProvider == loginProvider && u.ProviderKey == name)
                .Select(u => u.ProviderDisplayName)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch
        {
            return Task.FromResult<string?>(null);
        }
    }

    public async Task RemoveTokenAsync(User user, string loginProvider, string name,
        CancellationToken cancellationToken)
    {
        try
        {
            UserLogin? userLogin = await _context.UserLogins.Where(u =>
                    u.UserId == user.Id && u.LoginProvider == loginProvider && u.ProviderKey == name)
                .FirstOrDefaultAsync(cancellationToken);

            if (userLogin == null)
            {
                return;
            }

            _context.UserLogins.Remove(userLogin);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
        }
    }

    public Task SetTokenAsync(User user, string loginProvider, string name, string? value,
        CancellationToken cancellationToken)
    {
        try
        {
            UserLogin userLogin = new()
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                ProviderKey = name,
                ProviderDisplayName = value ?? ""
            };

            _context.UserLogins.Add(userLogin);

            return _context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return Task.CompletedTask;
        }
    }

    public Task<string?> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<UserClaim> c = claims.Select(x =>
                new UserClaim { UserId = user.Id, ClaimType = x.Type, ClaimValue = x.Value });

            _context.AddRangeAsync(c, cancellationToken);

            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            return Task.FromException(e);
        }
    }

    public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            List<Claim> claims = _context.UserClaims.Select(x => new Claim(x.ClaimType!, x.ClaimValue!))
                .ToList();

            return Task.FromResult<IList<Claim>>(claims);
        }
        catch (Exception e)
        {
            return Task.FromException<IList<Claim>>(e);
        }
    }

    public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        try
        {
            List<User> users = _context.UserClaims.Where(x => x.ClaimType == claim.Type &&
                                                              x.ClaimValue == claim.Value)
                .Select(x => x.User)
                .ToList();

            return Task.FromResult<IList<User>>(users);
        }
        catch (Exception e)
        {
            return Task.FromException<IList<User>>(e);
        }
    }

    public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        try
        {
            user.Claims.RemoveAll(x => claims.Any(c => c.Type == x.ClaimType && c.Value == x.ClaimValue));

            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            return Task.FromException(e);
        }
    }

    public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        try
        {
            user.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            user.Claims.Add(new UserClaim { UserId = user.Id, ClaimType = newClaim.Type, ClaimValue = newClaim.Value });

            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            return Task.FromException(e);
        }
    }


    public Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(_context.Users.FirstOrDefault(u => u.NormalizedEmail == normalizedEmail));
        }
        catch
        {
            return Task.FromResult<User?>(null);
        }
    }

    public Task<string?> GetEmailAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email = email);
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed = confirmed);
    }

    public Task SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail = normalizedEmail);
    }

    public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.AccessFailedCount);
    }

    public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.LockoutEnabled);
    }

    public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.LockoutEnd);
    }

    public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.AccessFailedCount++);
    }

    public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.AccessFailedCount = 0);
    }

    public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.LockoutEnabled = enabled);
    }

    public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.LockoutEnd = lockoutEnd);
    }

    public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        try
        {
            UserLogin userLogin = new()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                ProviderDisplayName = login.ProviderDisplayName
            };

            user.Logins.Add(userLogin);

            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            return Task.FromException(e);
        }
    }

    public async Task<User?> FindByLoginAsync(string loginProvider, string providerKey,
        CancellationToken cancellationToken)
    {
        try
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Logins
                .Any(x => x.LoginProvider == loginProvider &&
                          x.ProviderKey == providerKey));

            return user;
        }
        catch
        {
            return null;
        }
    }

    public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
    {
        List<UserLoginInfo> logins = user.Logins
            .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))
            .ToList();

        return Task.FromResult<IList<UserLoginInfo>>(logins);
    }

    public Task RemoveLoginAsync(User user, string loginProvider, string providerKey,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Logins.RemoveAll(x =>
            x.LoginProvider == loginProvider && x.ProviderKey == providerKey));
    }

    public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash != null);
    }

    public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash = passwordHash);
    }

    public Task<string?> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneNumber);
    }

    public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    public Task SetPhoneNumberAsync(User user, string? phoneNumber, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneNumber = phoneNumber);
    }

    public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneNumberConfirmed = confirmed);
    }

    public Task<string?> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.SecurityStamp);
    }

    public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.SecurityStamp = stamp);
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            _context.Add(user);

            int result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError { Description = "Failed to create user." });
        }
        catch (Exception e)
        {
            return IdentityResult.Failed(new IdentityError { Description = e.Message });
        }
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            _context.Remove(user);

            int result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError { Description = "Failed to delete user." });
        }
        catch (Exception e)
        {
            return IdentityResult.Failed(new IdentityError { Description = e.Message });
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName = normalizedName);
    }

    public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName = userName);
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            _context.Update(user);

            int result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError { Description = "Failed to update user." });
        }
        catch (Exception e)
        {
            return IdentityResult.Failed(new IdentityError { Description = e.Message });
        }
    }

    Task<User?> IUserStore<User>.FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            Guid id = Guid.Parse(userId);

            return _context.Users.FirstOrDefaultAsync(x => x.Id == id,
                cancellationToken);
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

    public Task<int> CountCodesAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.TwoFactorEnabled);
    }

    public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.TwoFactorEnabled = enabled);
    }

    public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            Guid id = Guid.Parse(userId);

            return _context.Users.FirstOrDefaultAsync(x => x.Id == id,
                cancellationToken);
        }
        catch (Exception)
        {
            return Task.FromResult<User?>(null);
        }
    }

    public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
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