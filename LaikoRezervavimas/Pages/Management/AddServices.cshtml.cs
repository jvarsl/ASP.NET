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
    public class AddServicesModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AddServicesModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public string CurrentFilter { get; set; }
        public PaginatedList<Service> Service { get; set; }

        public async Task<IActionResult> OnGetAsync(string currentFilter, string searchString, int? pageIndex)
        {
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;

            }
            CurrentFilter = searchString;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var staffManagesSalonIds = await _context.StaffSalons
                .Where(r => r.StaffId == user.Id).Select(r => r.SalonId).ToListAsync();


            IQueryable<Service> appUserIQ = _context.Services
                .Include(r => r.Salon)
                .Include(s => s.ServiceCategory)
                .Where(r => r.Active)
                .Where(r => staffManagesSalonIds.Contains(r.Salon.Id));

            int x = 0;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (Int32.TryParse(searchString, out x))
                {
                    appUserIQ = appUserIQ.Where(s => s.Id.Equals(x));
                }
                else
                {
                    appUserIQ = appUserIQ.Where(s => s.Name.Contains(searchString) || s.Salon.Name.Contains(searchString));
                }
            }

            int pageSize = 20;
            Service = await PaginatedList<Service>.CreateAsync(
                appUserIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }

    }
}