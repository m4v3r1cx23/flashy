using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.Decks;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public InputModel InputModel { get; set; } = default!;

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

        InputModel = new InputModel
        {
            Id = deck.Id, Name = deck.Name, Description = deck.Description, CreatedAt = deck.CreatedAt
        };
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (InputModel.Id == null)
        {
            return NotFound();
        }

        Deck? deck = await _context.Decks.FirstOrDefaultAsync(m => m.Id == InputModel.Id);
        if (deck == null)
        {
            return NotFound();
        }

        deck.Name = InputModel.Name;
        deck.NormalizedName = InputModel.Name.Normalize();
        deck.Description = InputModel.Description;
        deck.CreatedAt = InputModel.CreatedAt ?? deck.CreatedAt;

        _context.Attach(deck).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DeckExists(InputModel.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool DeckExists(Guid? id)
    {
        return id != null && _context.Decks.Any(e => e.Id == id);
    }
}