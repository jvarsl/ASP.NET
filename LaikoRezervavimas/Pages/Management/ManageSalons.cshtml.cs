using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Staff")]
    public class ManageSalonsModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public ManageSalonsModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public AppUser AppUser { get; set; }
        public IList<Salon> Salon { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            AppUser = await _context.AppUsers.Include(r => r.AppUserRole).ThenInclude(r => r.Role).Where(r => r.Id == user.Id).FirstOrDefaultAsync();

            if (await _userManager.IsInRoleAsync(user, "Staff"))
            {
                Salon = await _context.Salons
                .Include(r => r.StaffSalon.Where(r => r.StaffId == user.Id))
                .Include(r => r.City)
                .Include(r => r.Company)
                .Where(r => r.StaffSalon.Select(e=>e.StaffId).Contains(user.Id))
                .ToListAsync();
            }

            return Page();
        }

    }
}