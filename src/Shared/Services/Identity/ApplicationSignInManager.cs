using Flashy.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Flashy.Shared.Services.Identity;

public class ApplicationSignInManager : SignInManager<User>
{
  public ApplicationSignInManager(UserManager<User> userManager,
                                  IHttpContextAccessor contextAccessor,
                                  IUserClaimsPrincipalFactory<User> claimsFactory,
                                  IOptions<IdentityOptions> optionsAccessor,
                                  ILogger<SignInManager<User>> logger,
                                  IAuthenticationSchemeProvider schemes,
                                  IUserConfirmation<User> confirmation) : base(userManager,
                                                                               contextAccessor,
                                                                               claimsFactory,
                                                                               optionsAccessor,
                                                                               logger,
                                                                               schemes,
                                                                               confirmation)
  { }
}