namespace Flashy.API.Models.Auth;

public class ChangePasswordModel
{
  public string CurrentPassword { get; set; }
  public string NewPassword { get; set; }
}
