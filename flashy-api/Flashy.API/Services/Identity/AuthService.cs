using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Flashy.API.Services.Identity;

public class AuthService(IConfiguration configuration)
{
  private const int EXPIRES_IN_MINUTES = 30;

  private readonly IConfiguration _configuration = configuration;

  public string GenerateToken(User user)
  {
    var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ??
        throw new InvalidOperationException("JWT Key not found."));
    var issuer = _configuration["JWT:Issuer"] ??
      throw new InvalidOperationException("JWT Issuer not found.");
    var audience = _configuration["JWT:Audience"] ??
      throw new InvalidOperationException("JWT Issuer not found.");

    var tokenDescriptor = new JwtSecurityToken
    (
      issuer: issuer,
      audience: audience,
      claims: GenerateClaims(user),
      expires: DateTime.Now.AddMinutes(EXPIRES_IN_MINUTES),
      signingCredentials: new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
    );

    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
  }

  private static List<Claim> GenerateClaims(User user)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new(ClaimTypes.Name, user.UserName),
      new(ClaimTypes.Email, user.Email),
      new(ClaimTypes.GivenName, user.FullName),
    };

    claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

    return claims;
  }
}
