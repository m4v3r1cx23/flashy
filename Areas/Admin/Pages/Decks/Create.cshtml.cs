using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Flashy.Data.Context;
using Flashy.Shared.Models;

namespace Flashy.Areas.Admin.Pages.Decks
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

        [BindProperty]
        public InputModel InputModel { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var userName = HttpContext.User.Identity?.Name;

            if (userName == null)
            {
                return Page();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == userName);

            if (user == null)
            {
                return Page();
            }

            var deck = new Deck()
            {
                Id = Guid.NewGuid(),
                Name = InputModel.Name,
                NormalizedName = InputModel.Name.Normalize(),
                Description = InputModel.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = user,
            };

            _context.Decks.Add(deck);
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

        [StringLength(128, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
        
        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }
    }
}
