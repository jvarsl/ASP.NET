using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Admin")]
    public class AssignStaffToSalonsModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        public AssignStaffToSalonsModel(SalonWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public string CurrentFilter { get; set; }
        public PaginatedList<Employee> AppUser { get; set; }

        public async Task OnGetAsync(string sortOrder,
    string currentFilter, string searchString, int? pageIndex)
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
            IQueryable<Employee> appUserIQ = _context.Employees
                .Include(r => r.AppUserRole)
                .ThenInclude(r => r.Role)
                .Where(r=>r.AppUserRole.Where(e=>e.RoleId==3).Any());

            int x = 0;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (Int32.TryParse(searchString, out x))
                {
                    appUserIQ = appUserIQ.Where(s => s.Id.Equals(x));
                }
                else
                {
                    appUserIQ = appUserIQ.Where(s => s.FirstName.Contains(searchString)
                        || s.LastName.Contains(searchString));
                }
            }

            int pageSize = 20;
            AppUser = await PaginatedList<Employee>.CreateAsync(
                appUserIQ.AsNoTracking(), pageIndex ?? 1, pageSize);


            
        }
    }
}
