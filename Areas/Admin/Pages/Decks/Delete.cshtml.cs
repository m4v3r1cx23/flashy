using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.Decks;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public Deck Deck { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Deck? deck = await _context.Decks.FirstOrDefaultAsync(m => m.Id == id);

        if (deck == null)
        {
            return NotFound();
        }

        Deck = deck;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Deck? deck = await _context.Decks.FindAsync(id);
        if (deck != null)
        {
            Deck = deck;
            _context.Decks.Remove(Deck);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}