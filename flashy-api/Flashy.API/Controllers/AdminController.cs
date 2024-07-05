using Flashy.API.Models;
using Flashy.API.Models.Admin;
using Flashy.API.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashy.API.Controllers
{
  [Authorize(Roles = "Admin")]
  [Route("[controller]")]
  public class AdminController(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<AdminController> logger) : Controller
  {
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly ILogger<AdminController> _logger = logger;

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
      var usersQuery = _userManager.Users.Select(user => new
      {
        user.Id,
        user.Email,
        user.UserName,
        user.FirstName,
        user.LastName,
        Roles = user.Roles.Select(r => r.Name),
      });

      var totalUsers = await usersQuery.CountAsync();
      var users = await usersQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

      var pagedResult = new PageResult
      {
        TotalCount = totalUsers,
        PageNumber = pageNumber,
        PageSize = pageSize,
        Data = users
      };

      return Ok(pagedResult);
    }

    [HttpPost("change-password/{userId}")]
    public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangeUserPasswordModel model)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
      var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }

    [HttpPut("update/{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserModel model)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      user.FirstName = model.FirstName;
      user.LastName = model.LastName;

      var result = await _userManager.UpdateAsync(user);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }

    [HttpPost("change-roles/{userId}")]
    public async Task<IActionResult> ChangeRoles(string userId, [FromBody] ChangeRolesModel model)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      var userRoles = await _userManager.GetRolesAsync(user);
      var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      result = await _userManager.AddToRolesAsync(user, model.Roles);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }

    [HttpPost("activate/{userId}")]
    public async Task<IActionResult> ActivateUser(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      user.LockoutEnd = null;
      var result = await _userManager.UpdateAsync(user);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }

    [HttpPost("deactivate/{userId}")]
    public async Task<IActionResult> DeactivateUser(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      user.LockoutEnd = DateTimeOffset.MaxValue;
      var result = await _userManager.UpdateAsync(user);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      var result = await _userManager.DeleteAsync(user);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
      var rolesQuery = _roleManager.Roles.Select(role => new
      {
        role.Id,
        role.Name
      });

      var totalRoles = await rolesQuery.CountAsync();
      var roles = await rolesQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

      var pagedResult = new PageResult
      {
        TotalCount = totalRoles,
        PageNumber = pageNumber,
        PageSize = pageSize,
        Data = roles
      };

      return Ok(pagedResult);
    }

    [HttpPost("roles")]
    public async Task<IActionResult> AddRole([FromBody] RoleModel model)
    {
      if (await _roleManager.RoleExistsAsync(model.Name))
      {
        return BadRequest("Role already exists.");
      }

      var role = new Role
      {
        Name = model.Name,
        NormalizedName = model.Name.Normalize(),
        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
      };
      var result = await _roleManager.CreateAsync(role);

      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }

    [HttpDelete("roles/{roleName}")]
    public async Task<IActionResult> RemoveRole(string roleName)
    {
      var role = await _roleManager.FindByNameAsync(roleName);
      if (role == null)
      {
        return NotFound("Role not found.");
      }

      var result = await _roleManager.DeleteAsync(role);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }
  }
}
