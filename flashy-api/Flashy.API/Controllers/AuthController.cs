using Flashy.API.Models.Auth;
using Flashy.API.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flashy.API.Controllers
{
  [Authorize]
  [Route("[controller]")]
  public class AuthController(AuthService authService,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              ILogger<AuthController> logger) : Controller
  {
    private readonly AuthService _authService = authService;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly ILogger<AuthController> _logger = logger;

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var user = new User
      {
        UserName = model.Username,
        NormalizedUserName = model.Username.Normalize(),
        Email = model.Email,
        NormalizedEmail = model.Email.Normalize(),
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString("D"),
        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
        FirstName = model.FirstName,
        LastName = model.LastName,
      };

      var result = await _userManager.CreateAsync(user, model.Password);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      var roleResult = await _userManager.AddToRoleAsync(user, "User");
      if (!roleResult.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      // Automatically sign in the user after registration
      await _signInManager.SignInAsync(user, isPersistent: true);

      var token = _authService.GenerateToken(user);

      return Ok(new { token });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
      if (string.IsNullOrEmpty(model.Email))
      {
        return BadRequest();
      }

      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null)
      {
        return Unauthorized();
      }

      var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
      if (!result.Succeeded)
      {
        return Unauthorized();
      }

      var token = _authService.GenerateToken(user);

      return Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();

      return Ok();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel model)
    {
      var user = await _userManager.GetUserAsync(User);
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

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUser()
    {
      var user = await _userManager.GetUserAsync(User);
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
  }
}
