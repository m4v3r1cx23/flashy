using Flashy.Data.Context;
using Flashy.Shared.Models.Identity;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.Roles;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Role> Role { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Role = await _context.Roles.ToListAsync();
    }
}