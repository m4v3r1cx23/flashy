using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.FlashCards;

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

        FlashCard? flashcard = await _context.Flashcards.FirstOrDefaultAsync(m => m.Id == id);
        if (flashcard == null)
        {
            return NotFound();
        }

        InputModel = new InputModel
        {
            Id = flashcard.Id,
            Front = flashcard.Front,
            Back = flashcard.Back,
            Hint = flashcard.Hint,
            CreatedAt = flashcard.CreatedAt
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

        FlashCard? flashcard = await _context.Flashcards.FirstOrDefaultAsync(m => m.Id == InputModel.Id);
        if (flashcard == null)
        {
            return NotFound();
        }

        flashcard.Front = InputModel.Front;
        flashcard.NormalizedFront = InputModel.Front.Normalize();
        flashcard.Back = InputModel.Back;
        flashcard.Back = InputModel.Back.Normalize();
        flashcard.Hint = InputModel.Hint;
        flashcard.CreatedAt = InputModel.CreatedAt ?? flashcard.CreatedAt;

        _context.Attach(flashcard).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FlashCardExists(InputModel.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool FlashCardExists(Guid? id)
    {
        return id != null && _context.Flashcards.Any(e => e.Id == id);
    }
}