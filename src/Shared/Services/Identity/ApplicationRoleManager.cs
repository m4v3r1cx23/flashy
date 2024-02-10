using Flashy.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Flashy.Shared.Services.Identity;

public class ApplicationRoleManager : RoleManager<Role>
{
  public ApplicationRoleManager(IRoleStore<Role> store,
                                IEnumerable<IRoleValidator<Role>> roleValidators,
                                ILookupNormalizer keyNormalizer,
                                IdentityErrorDescriber errors,
                                ILogger<RoleManager<Role>> logger) : base(store,
                                                                          roleValidators,
                                                                          keyNormalizer,
                                                                          errors,
                                                                          logger)
  { }
}