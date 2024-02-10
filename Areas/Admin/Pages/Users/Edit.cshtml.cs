using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Flashy.Data.Context;
using Flashy.Shared.Models;

using Microsoft.AspNetCore.Identity;

namespace Flashy.Areas.Admin.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly Flashy.Data.Context.ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public EditModel(Flashy.Data.Context.ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [BindProperty] public InputModel InputModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            InputModel = new InputModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnd = user.LockoutEnd,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,
            };
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

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == InputModel.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = InputModel.FirstName;
            user.LastName = InputModel.LastName;
            user.UserName = InputModel.UserName;
            user.NormalizedUserName = InputModel.UserName.Normalize();
            user.Email = InputModel.Email;
            user.NormalizedEmail = InputModel.Email.Normalize();
            user.EmailConfirmed = InputModel.EmailConfirmed;
            user.PhoneNumber = InputModel.PhoneNumber;
            user.PhoneNumberConfirmed = InputModel.PhoneNumberConfirmed;
            user.TwoFactorEnabled = InputModel.TwoFactorEnabled;
            user.LockoutEnabled = InputModel.LockoutEnabled;
            user.LockoutEnd = InputModel.LockoutEnd;
            user.AccessFailedCount = InputModel.AccessFailedCount;

            if (!string.IsNullOrEmpty(InputModel.Password))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.PasswordHash = _passwordHasher.HashPassword(user, InputModel.Password);
            }

            _context.Attach(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(InputModel.Id))
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

        private bool UserExists(Guid? id)
        {
            return id != null && _context.Users.Any(e => e.Id == id);
        }
    }
}