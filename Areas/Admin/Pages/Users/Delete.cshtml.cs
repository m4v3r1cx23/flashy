using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flashy.Areas.Admin.Pages.Users;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public User User { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        User? user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        User = user;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        User? user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            User = user;
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}