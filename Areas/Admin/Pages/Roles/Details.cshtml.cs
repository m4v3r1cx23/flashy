using Flashy.Data.Context;
using Flashy.Shared.Models.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.Roles;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Role Role { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Role? role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
        if (role == null)
        {
            return NotFound();
        }

        Role = role;
        return Page();
    }
}