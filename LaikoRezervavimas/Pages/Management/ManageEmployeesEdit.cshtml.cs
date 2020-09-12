using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public class ManageEmployeesEditModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ManageEmployeesEditModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Employee Employee { get; set; }

        public class EmployeeViewModel
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            [Display(Name = "Profesija")]
            public int? JobTitleId { get; set; }
            public int SalonId { get; set; }
            public List<EmployeeSchedule> EmployeeSchedule { get; set; }
            public List<SalonSchedule> SalonSchedule { get; set; }
        }

        [BindProperty]
        public EmployeeViewModel EmployeeVM { get; set; }


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

            Employee = await _context.Employees
               .Include(e => e.JobTitle)
               .Include(e => e.EmployeeSchedule)
               .Include(r => r.Salon)
                .ThenInclude(r => r.SalonSchedule)
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.Id == id);

            if (Employee == null || !staffManagesSalonIds.Contains(Employee.SalonId.Value))
            {
                return NotFound();
            }
            EmployeeVM = new EmployeeViewModel
            {
                Id = Employee.Id,
                FullName = Employee.FullName,
                JobTitleId = Employee.JobTitleId,
                SalonId = Employee.SalonId.Value,
                EmployeeSchedule = Employee.EmployeeSchedule.ToList(),
                SalonSchedule = Employee.Salon.SalonSchedule.ToList()
            };
            if (EmployeeVM.JobTitleId.HasValue)
            {
                ViewData["JobTitleId"] = new SelectList(_context.JobTitles, "Id", "Name", EmployeeVM.JobTitleId.Value);
            }
            else
            {
                ViewData["JobTitleId"] = new SelectList(_context.JobTitles, "Id", "Name");
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

            var staffManagesSalonIds = await _context.StaffSalons
              .Where(r => r.StaffId == user.Id).Select(r => r.SalonId).ToListAsync();

            if (!staffManagesSalonIds.Contains(EmployeeVM.SalonId))
            {
                return NotFound();
            }

            foreach (var day in EmployeeVM.EmployeeSchedule)
            {
                if (day.EndTime.Value.Minutes % 15 != 0 || day.StartTime.Value.Minutes % 15 != 0
                    || day.BreakEndTime.Value.Minutes % 15 != 0 || day.BreakStartTime.Value.Minutes % 15 != 0)
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
                    day.BreakStartTime = new TimeSpan(0, 0, 0);
                    day.BreakEndTime = new TimeSpan(0, 0, 0);
                    day.IsTakingBreak = false;
                }
                else if (!day.IsTakingBreak)
                {
                    day.BreakStartTime = new TimeSpan(0, 0, 0);
                    day.BreakEndTime = new TimeSpan(0, 0, 0);
                }
                if (day.EndTime < day.StartTime)
                {
                    StatusMessage = $"Error: Darbo laiko pradžia yra vėlesė nei pabaigos. Diena: {Tools.DayToWord.LithuanianDayWord(day.Day)}.";
                    return Page();
                }
                if (day.BreakEndTime < day.BreakStartTime)
                {
                    StatusMessage = $"Error: Petraukos laikas prasideda vėliau, nei pabaiga. Diena: {Tools.DayToWord.LithuanianDayWord(day.Day)}.";
                    return Page();
                }
                if (day.IsTakingBreak && (day.StartTime > day.BreakStartTime || day.EndTime < day.BreakEndTime))
                {
                    StatusMessage = $"Error: Petraukos laikas nėra tarp darbo laiko. Diena: {Tools.DayToWord.LithuanianDayWord(day.Day)}.";
                    return Page();
                }
                if (day.IsWorking && (day.StartTime < EmployeeVM.SalonSchedule[day.Day.Value - 1].StartTime || day.EndTime > EmployeeVM.SalonSchedule[day.Day.Value - 1].EndTime))
                {
                    StatusMessage = $"Error: Darbuotojo darbo laikas nėra tarp salono darbo laiko. Diena: {Tools.DayToWord.LithuanianDayWord(day.Day)}.";
                    return Page();
                }
            }
            Employee = await _context.Employees
               .Include(e => e.EmployeeSchedule)
               .FirstOrDefaultAsync(m => m.Id == EmployeeVM.Id);

            Employee.JobTitleId = EmployeeVM.JobTitleId;
            int index = 0;
            foreach (var day in Employee.EmployeeSchedule)
            {
                if (day.Day == EmployeeVM.EmployeeSchedule[index].Day)
                {
                    day.IsTakingBreak = EmployeeVM.EmployeeSchedule[index].IsTakingBreak;
                    day.IsWorking = EmployeeVM.EmployeeSchedule[index].IsWorking;
                    day.StartTime = EmployeeVM.EmployeeSchedule[index].StartTime;
                    day.BreakStartTime = EmployeeVM.EmployeeSchedule[index].BreakStartTime;
                    day.EndTime = EmployeeVM.EmployeeSchedule[index].EndTime;
                    day.BreakEndTime = EmployeeVM.EmployeeSchedule[index].BreakEndTime;
                }
                index++;
            }
            var test = Employee.EmployeeSchedule.Where(r => r.Day == 5).FirstOrDefault();
            _context.Update(Employee);
            await _context.SaveChangesAsync();


            return RedirectToPage("./ManageEmployees");
        }

    }
}