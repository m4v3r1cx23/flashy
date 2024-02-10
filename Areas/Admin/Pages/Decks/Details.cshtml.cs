using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Flashy.Data.Context;
using Flashy.Shared.Models;

namespace Flashy.Areas.Admin.Pages.Decks
{
    public class DetailsModel : PageModel
    {
        private readonly Flashy.Data.Context.ApplicationDbContext _context;

        public DetailsModel(Flashy.Data.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        public Deck Deck { get; set; } = default!;

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
            else
            {
                Deck = deck;
            }
            return Page();
        }
    }
}
