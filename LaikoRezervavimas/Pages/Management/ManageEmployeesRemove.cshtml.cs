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
    public class ManageEmployeesRemoveModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public ManageEmployeesRemoveModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = await _context.Employees
                .Include(s => s.Salon).FirstOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(User);
            var staffManagesSalonIds = await _context.StaffSalons
                    .Where(r => r.StaffId == user.Id).Select(r => r.SalonId).ToListAsync();
            if (Employee == null || !staffManagesSalonIds.Contains(Employee.Salon.Id))
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

            Employee = await _context.Employees.Include(r => r.Salon).Where(r => r.Id == id).FirstOrDefaultAsync();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var staffManagesSalonIds = await _context.StaffSalons
                .Where(r => r.StaffId == user.Id).Select(r => r.SalonId).ToListAsync();
            if (Employee != null && staffManagesSalonIds.Contains(Employee.Salon.Id))
            {
                Employee.Salon = null;
                Employee.JobTitleId = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ManageEmployees");
        }
    }
}