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
    public class AddServicesMarkInactiveModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AddServicesMarkInactiveModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public Service Service { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service = await _context.Services
                .Include(s => s.ServiceCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (Service == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service = await _context.Services.Include(r=>r.Salon).Where(r=>r.Id==id).FirstOrDefaultAsync();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var staffManagesSalonIds = await _context.StaffSalons
                .Where(r => r.StaffId == user.Id).Select(r => r.SalonId).ToListAsync();
            if (Service != null && staffManagesSalonIds.Contains(Service.Salon.Id))
            {
                Service.Active = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./AddServices");
        }
    }
}
