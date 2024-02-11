using Flashy.Data.Context;
using Flashy.Shared.Models.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.Roles;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public Role Role { get; set; } = default!;

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

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Role? role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            Role = role;
            _context.Roles.Remove(Role);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}