using System.ComponentModel.DataAnnotations;

using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flashy.Areas.Admin.Pages.Users;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public CreateModel(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [BindProperty] public InputModel InputModel { get; set; } = default!;

    public IActionResult OnGet()
    {
        return Page();
    }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (InputModel.Password == null)
        {
            ModelState.AddModelError("Password", "Password can't be empty while creating new user!");
            return Page();
        }

        User user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = InputModel.FirstName,
            LastName = InputModel.LastName,
            UserName = InputModel.UserName,
            NormalizedUserName = InputModel.UserName.Normalize(),
            Email = InputModel.Email,
            NormalizedEmail = InputModel.Email.Normalize(),
            EmailConfirmed = InputModel.EmailConfirmed,
            PhoneNumber = InputModel.PhoneNumber,
            PhoneNumberConfirmed = InputModel.PhoneNumberConfirmed,
            TwoFactorEnabled = InputModel.TwoFactorEnabled,
            LockoutEnabled = InputModel.LockoutEnabled,
            LockoutEnd = InputModel.LockoutEnd,
            AccessFailedCount = InputModel.AccessFailedCount,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, InputModel.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

public class InputModel
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    [DataType(DataType.Text)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    [DataType(DataType.Text)]
    [Display(Name = "UserName")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(128, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Email Confirmed")]
    public bool EmailConfirmed { get; set; }

    [Display(Name = "Password")] public string? Password { get; set; }

    [Phone]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required]
    [Display(Name = "Phone Number Confirmed")]
    public bool PhoneNumberConfirmed { get; set; }

    [Required]
    [Display(Name = "Two Factor Enabled")]
    public bool TwoFactorEnabled { get; set; }

    [Display(Name = "Lockout End")] public DateTimeOffset? LockoutEnd { get; set; }

    [Required]
    [Display(Name = "Lockout Enabled")]
    public bool LockoutEnabled { get; set; } = true;

    [Display(Name = "Access Failed End")] public int AccessFailedCount { get; set; }
}