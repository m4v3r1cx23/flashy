using Microsoft.AspNetCore.Identity;

namespace Flashy.API.Infrastructure.Data.Migrations;

public class SeedData
{
  public static async Task InitializeAdmin(UserManager<User> userManager, RoleManager<Role> roleManager)
  {
    var adminRoleName = "Admin";
    var adminUserName = "admin@flashy.com";
    var adminPassword = "Flashy@dmin062024!";
    var adminFirstName = "John";
    var adminLastName = "Doe";

    if (!await roleManager.RoleExistsAsync(adminRoleName))
    {
      var role = new Role
      {
        Name = adminRoleName,
        NormalizedName = adminRoleName.Normalize(),
        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
      };

      await roleManager.CreateAsync(role);
    }

    if (await userManager.FindByNameAsync(adminUserName) == null)
    {
      var adminUser = new User
      {
        UserName = adminUserName,
        NormalizedUserName = adminUserName.Normalize(),
        Email = adminUserName,
        NormalizedEmail = adminUserName.Normalize(),
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString("D"),
        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
        FirstName = adminFirstName,
        LastName = adminLastName,
      };

      var result = await userManager.CreateAsync(adminUser, adminPassword);
      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(adminUser, adminRoleName);
      }
    }

    var adminUserCheck = await userManager.FindByNameAsync(adminUserName);
    if (adminUserCheck != null && !await userManager.IsInRoleAsync(adminUserCheck, adminRoleName))
    {
      await userManager.AddToRoleAsync(adminUserCheck, adminRoleName);
    }
  }

  public static async Task InitializeUser(UserManager<User> userManager, RoleManager<Role> roleManager)
  {
    var userRoleName = "User";
    var userName = "user@flashy.com";
    var userPassword = "FlashyUser062024!";
    var userFirstName = "John";
    var userLastName = "Doe";

    if (!await roleManager.RoleExistsAsync(userRoleName))
    {
      var role = new Role
      {
        Name = userRoleName,
        NormalizedName = userRoleName.Normalize(),
        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
      };

      await roleManager.CreateAsync(role);
    }

    if (await userManager.FindByNameAsync(userName) == null)
    {
      var user = new User
      {
        UserName = userName,
        NormalizedUserName = userName.Normalize(),
        Email = userName,
        NormalizedEmail = userName.Normalize(),
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString("D"),
        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
        FirstName = userFirstName,
        LastName = userLastName,
      };

      var result = await userManager.CreateAsync(user, userPassword);
      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(user, userRoleName);
      }
    }

    var userCheck = await userManager.FindByNameAsync(userName);
    if (userCheck != null && !await userManager.IsInRoleAsync(userCheck, userRoleName))
    {
      await userManager.AddToRoleAsync(userCheck, userRoleName);
    }
  }
}
