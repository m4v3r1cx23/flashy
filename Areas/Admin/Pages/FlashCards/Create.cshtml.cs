using System.ComponentModel.DataAnnotations;

using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flashy.Areas.Admin.Pages.FlashCards;

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

        FlashCard flashCard = new FlashCard
        {
            Id = Guid.NewGuid(),
            Front = InputModel.Front,
            NormalizedFront = InputModel.Front.Normalize(),
            Back = InputModel.Back,
            NormalizedBack = InputModel.Back.Normalize(),
            CreatedAt = DateTime.Now,
            CreatedBy = user
        };

        _context.Flashcards.Add(flashCard);
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
    [Display(Name = "Front")]
    public string Front { get; set; }

    [Required]
    [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    [DataType(DataType.Text)]
    [Display(Name = "Back")]
    public string Back { get; set; }

    [StringLength(128, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    [Display(Name = "Hint")]
    public string? Hint { get; set; }

    [Display(Name = "Created At")] public DateTime? CreatedAt { get; set; }
}