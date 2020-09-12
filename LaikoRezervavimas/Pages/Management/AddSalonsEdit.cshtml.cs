using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Admin")]
    public class AddSalonsEditModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        public AddSalonsEditModel(SalonWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Salon Salon { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Salon = await _context.Salons
                .Include(s => s.City)
                .Include(s => s.Company).FirstOrDefaultAsync(m => m.Id == id);

            if (Salon == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Salon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonExists(Salon.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./AddSalons");
        }

        private bool SalonExists(int id)
        {
            return _context.Salons.Any(e => e.Id == id);
        }
    }
}