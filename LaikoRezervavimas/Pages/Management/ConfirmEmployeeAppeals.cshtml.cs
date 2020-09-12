using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Migrations;
using SalonWithRazor.Models;


namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Staff")]
    public class ConfirmEmployeeAppealsModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public ConfirmEmployeeAppealsModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IList<EmployeeAppealSalon> EmployeeAppealSalons { get; set; }

        [BindProperty]
        public EmployeeAppealSalon EmployeeAppealSalon { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var staffManagesSalonIds = await _context.StaffSalons
                .Where(r => r.StaffId == user.Id).Select(r => r.SalonId).ToListAsync();

            EmployeeAppealSalons = await _context.EmployeeAppealSalons
                .Include(r => r.Employee)
                .Include(r => r.Salon)
                    .ThenInclude(r => r.City)
                .Where(r => staffManagesSalonIds.Contains(r.SalonId))
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync(int? id)
        {
            if (id == null)
            {
                StatusMessage = "Error: Prašymas nerastas";
                return Page();
            }

            EmployeeAppealSalon employeeAppealSalon = await _context.EmployeeAppealSalons.Where(r => r.Id == id.Value).FirstOrDefaultAsync();

            if (employeeAppealSalon == null)
            {
                StatusMessage = "Error: Prašymas nerastas";
                return Page();
            }

            var user = await _context.Employees.Where(r => r.Id == employeeAppealSalon.EmployeeId).FirstOrDefaultAsync();
            if (user == null)
            {
                StatusMessage = "Error: Tokio darbuotojo nėra.";
                return Page();
            }
            if (user.SalonId != null)
            {
                StatusMessage = "Error: Darbuotojas turi saloną.";
                return Page();
            }

            var salon = await _context.Salons.Where(r => r.Id == employeeAppealSalon.SalonId).FirstOrDefaultAsync();
            if (salon == null)
            {
                StatusMessage = "Error: Tokio salono nėra.";
                return Page();
            }

            var employeeAppealSalons = await _context.EmployeeAppealSalons.Where(r => r.EmployeeId == employeeAppealSalon.EmployeeId).ToListAsync();
            _context.EmployeeAppealSalons.RemoveRange(employeeAppealSalons);
            user.Salon = salon;

            await _context.EmployeeSchedules.Where(r => r.EmployeeId == user.Id).ForEachAsync(day =>
            {
                day.StartTime = new TimeSpan(0, 0, 0);
                day.EndTime = new TimeSpan(0, 0, 0);
                day.BreakStartTime = new TimeSpan(0, 0, 0);
                day.BreakEndTime = new TimeSpan(0, 0, 0);
                day.IsTakingBreak = false;
                day.IsWorking = false;
            });
            await _context.SaveChangesAsync();


            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostCancelAsync(int? id)
        {
            if (id == null)
            {
                StatusMessage = "Error: Prašymas nerastas";
                return Page();
            }

            EmployeeAppealSalon employeeAppealSalon = await _context.EmployeeAppealSalons.Where(r => r.Id == id.Value).FirstOrDefaultAsync();

            if (employeeAppealSalon == null)
            {
                StatusMessage = "Error: Prašymas nerastas";
                return Page();
            }

            _context.EmployeeAppealSalons.Remove(employeeAppealSalon);
            await _context.SaveChangesAsync();


            return RedirectToPage();
        }
    }
}