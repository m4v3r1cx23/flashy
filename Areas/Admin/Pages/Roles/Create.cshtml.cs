using System.ComponentModel.DataAnnotations;

using Flashy.Data.Context;
using Flashy.Shared.Models.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flashy.Areas.Admin.Pages.Roles;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
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

        Role role = new Role
        {
            Id = Guid.NewGuid(),
            Name = InputModel.Name,
            NormalizedName = InputModel.Name.Normalize(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        _context.Roles.Add(role);
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
    [Display(Name = "Name")]
    public string Name { get; set; }
}