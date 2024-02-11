using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.FlashCards;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<FlashCard> FlashCard { get; set; } = default!;

    public async Task OnGetAsync()
    {
        FlashCard = await _context.Flashcards.ToListAsync();
    }
}