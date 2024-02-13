using flashy.src.Shared.Services;

using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Docs.Samples;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ExampleService _exampleService;

    [BindProperty] public Learn Learn { get; set; } = null!;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context, ExampleService exampleService)
    {
        _logger = logger;
        _context = context;
        _exampleService = exampleService;
    }

    public async Task<IActionResult> OnGet(int? index)
    {
        index ??= 0;
        
        var cards = await _context.Flashcards.ToListAsync();

        if (cards.Count < index + 1)
        {
            return Page();
        }

        Learn = new Learn()
        {
            Cards = cards,
            Card = cards[index.Value],
            Index = index.Value,
            IsFirst = index == 0,
            IsLast = index + 1 >= cards.Count,
            UserNames = await _exampleService.GetExample()
        };
        ViewData["routeInfo"] = PageContext.ToCtxStringP();

        return Page();
    }

    public async Task OnPostPrevious(int index)
    {
        var cards = _context.Flashcards.ToList();

        if (index <= 0)
        {
            return;
        }

        index--;

        Learn = new Learn()
        {
            Cards = cards,
            Card = cards[index],
            Index = index,
            IsFirst = index == 0,
            IsLast = index + 1 >= cards.Count,
            UserNames = await _exampleService.GetExample()
        };
    }

    public async Task OnPostNext(int index)
    {
        var cards = _context.Flashcards.ToList();

        if (index + 1 >= cards.Count)
        {
            return;
        }

        index++;

        Learn = new Learn()
        {
            Cards = cards,
            Card = cards[index],
            Index = index,
            IsFirst = index == 0,
            IsLast = index + 1 >= cards.Count,
            UserNames = await _exampleService.GetExample()
        };
    }
}

public class Learn
{
    public List<FlashCard> Cards { get; set; } = [];
    public FlashCard Card { get; set; } = null!;
    public int Index { get; set; }
    public bool IsFirst { get; set; }
    public bool IsLast { get; set; }
    public List<string> UserNames { get; set; } = [];
}