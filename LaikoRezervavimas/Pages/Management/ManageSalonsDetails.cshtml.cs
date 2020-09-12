using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Staff")]
    public class ManageSalonsDetailsModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ManageSalonsDetailsModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Salon Salon { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var staffManagesSalonIds = await _context.StaffSalons
                .Where(r => r.StaffId == user.Id).Select(r => r.SalonId).ToListAsync();

            Salon = await _context.Salons
                .Include(s => s.City)
                .Include(s => s.Company)
                .Include(r=>r.SalonSchedule)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Salon == null || !staffManagesSalonIds.Contains(Salon.Id))
            {
                return NotFound();
            }
            return Page();
        }
    }
}
