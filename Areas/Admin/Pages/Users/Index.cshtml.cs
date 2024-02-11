using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.Users;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<User> User { get; set; } = default!;

    public async Task OnGetAsync()
    {
        User = await _context.Users.ToListAsync();
    }
}