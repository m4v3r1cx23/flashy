using System.ComponentModel.DataAnnotations;

using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flashy.Areas.Admin.Pages.Decks;

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

        string? userName = HttpContext.User.Identity?.Name;

        if (userName == null)
        {
            return Page();
        }

        User? user = _context.Users.FirstOrDefault(u => u.Email == userName);

        if (user == null)
        {
            return Page();
        }

        Deck deck = new Deck
        {
            Id = Guid.NewGuid(),
            Name = InputModel.Name,
            NormalizedName = InputModel.Name.Normalize(),
            Description = InputModel.Description,
            CreatedAt = DateTime.Now,
            CreatedBy = user
        };

        _context.Decks.Add(deck);
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

    [StringLength(128, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    [DataType(DataType.Text)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Created At")] public DateTime? CreatedAt { get; set; }
}