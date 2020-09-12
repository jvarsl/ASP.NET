using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Staff")]
    public class ManageSalonsEditModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ManageSalonsEditModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Salon Salon { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
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
                .Include(r => r.SalonSchedule)
                //.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Salon == null || !staffManagesSalonIds.Contains(Salon.Id))
            {
                return NotFound();
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            //var salon = await _context.Salons.Where(r=>r.Id==Salon.Id).FirstOrDefaultAsync()
            var staffManagesSalonIds = await _context.StaffSalons
              .Where(r => r.StaffId == user.Id).Select(r => r.SalonId).ToListAsync();

            if (!staffManagesSalonIds.Contains(Salon.Id))
            {
                return NotFound();
            }

            foreach (var day in Salon.SalonSchedule)
            {
                if (day.EndTime.Value.Minutes % 15 != 0 || day.StartTime.Value.Minutes % 15 != 0)
                {
                    StatusMessage = "Error: Minutės gali būti tik 15 min intervalu (0,15,30,45).";
                    return Page();
                }
                if (day.EndTime == day.StartTime)
                {
                    day.IsWorking = false;
                }
                if (!day.IsWorking)
                {
                    day.StartTime = new TimeSpan(0, 0, 0);
                    day.EndTime = new TimeSpan(0, 0, 0);
                }
                if (day.EndTime < day.StartTime)
                {
                    StatusMessage = $"Error: Salono darbo laikas yra vėlesnis nei pabaigos. Diena: {Tools.DayToWord.LithuanianDayWord(day.Day)}.";
                    return Page();
                }
            }
            var employeeSchedule = await _context.EmployeeSchedules.Include(r => r.Employee).Where(r => r.Employee.SalonId == Salon.Id).ToListAsync();
            List<string> employeeNamesWithSchedulesChanged = new List<string>();
            foreach (var salonDay in Salon.SalonSchedule)
            {
                var takenDayListOfEmployeeSchedules = employeeSchedule.Where(r => r.Day == salonDay.Day);

                foreach (var employeeDay in takenDayListOfEmployeeSchedules)
                {
                    bool IsChangeMade = false;
                    if (employeeDay.StartTime < salonDay.StartTime)
                    {
                        employeeDay.StartTime = salonDay.StartTime;
                        IsChangeMade = true;
                    }
                    if (employeeDay.EndTime > salonDay.EndTime)
                    {
                        employeeDay.EndTime = salonDay.EndTime;
                        IsChangeMade = true;
                    }
                    if (IsChangeMade)
                    {
                        employeeNamesWithSchedulesChanged.Add(employeeDay.Employee.FullName);
                    }
                }
            }

            _context.Update(Salon);
            await _context.SaveChangesAsync();

            StatusMessage = !employeeNamesWithSchedulesChanged.Any() ? "Success: Salono darbo laikas atnaujintas"
            : $"Success: Salono darbo laikas atnaujintas ir pataisytas darbo laikas šiems darbuotojams: \r\n{String.Join(",", employeeNamesWithSchedulesChanged.Distinct())}";
            return Page();
            //return RedirectToPage("./ManageSalons");
        }

        private bool SalonExists(int id)
        {
            return _context.Salons.Any(e => e.Id == id);
        }
    }
}