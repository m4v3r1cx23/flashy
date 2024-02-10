using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Flashy.Data.Context;
using Flashy.Shared.Models.Identity;

namespace Flashy.Areas.Admin.Pages.Roles
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

            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            InputModel = new InputModel() { Id = role.Id, Name = role.Name ?? "", };
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

            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == InputModel.Id);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = InputModel.Name;
            role.NormalizedName = InputModel.Name.Normalize();

            _context.Attach(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(InputModel.Id))
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

        private bool RoleExists(Guid? id)
        {
            return id != null && _context.Roles.Any(e => e.Id == id);
        }
    }
}