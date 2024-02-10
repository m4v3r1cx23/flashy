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
    public class IndexModel : PageModel
    {
        private readonly Flashy.Data.Context.ApplicationDbContext _context;

        public IndexModel(Flashy.Data.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Deck> Deck { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Deck = await _context.Decks.ToListAsync();
        }
    }
}
