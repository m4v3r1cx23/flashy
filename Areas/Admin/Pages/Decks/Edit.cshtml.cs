using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Flashy.Data.Context;
using Flashy.Shared.Models;

namespace Flashy.Areas.Admin.Pages.Decks
{
    public class EditModel : PageModel
    {
        private readonly Flashy.Data.Context.ApplicationDbContext _context;

        public EditModel(Flashy.Data.Context.ApplicationDbContext context)
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

            var deck = await _context.Decks.FirstOrDefaultAsync(m => m.Id == id);
            if (deck == null)
            {
                return NotFound();
            }

            InputModel = new InputModel()
            {
                Id = deck.Id, Name = deck.Name, Description = deck.Description, CreatedAt = deck.CreatedAt,
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

            var deck = await _context.Decks.FirstOrDefaultAsync(m => m.Id == InputModel.Id);
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
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DeckExists(Guid? id)
        {
            return id != null && _context.Decks.Any(e => e.Id == id);
        }
    }
}