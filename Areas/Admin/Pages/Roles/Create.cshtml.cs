using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Flashy.Data.Context;
using Flashy.Shared.Models.Identity;

namespace Flashy.Areas.Admin.Pages.Roles
{
    public class CreateModel : PageModel
    {
        private readonly Flashy.Data.Context.ApplicationDbContext _context;

        public CreateModel(Flashy.Data.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty] public InputModel InputModel { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var role = new Role()
            {
                Id = Guid.NewGuid(), Name = InputModel.Name, NormalizedName = InputModel.Name.Normalize(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

    public class InputModel
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}