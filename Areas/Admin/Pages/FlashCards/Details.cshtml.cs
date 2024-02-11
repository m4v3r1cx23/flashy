using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.FlashCards;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public FlashCard FlashCard { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        FlashCard? flashcard = await _context.Flashcards.FirstOrDefaultAsync(m => m.Id == id);
        if (flashcard == null)
        {
            return NotFound();
        }

        FlashCard = flashcard;
        return Page();
    }
}