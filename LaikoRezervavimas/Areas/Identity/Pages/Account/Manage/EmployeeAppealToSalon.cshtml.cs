using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Models;

namespace SalonWithRazor.Areas.Identity.Pages.Account.Manage
{
    public class EmployeeAppealToSalonModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<Employee> _userManager;
        public EmployeeAppealToSalonModel(
            SalonWithRazor.Data.ApplicationDbContext context,
            UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IList<EmployeeAppealSalon> EmployeeAppealSalons { get; set; }

        [BindProperty]
        public EmployeeAppealSalon EmployeeAppealSalon { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public Employee user { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            user = await _userManager.GetUserAsync(User);
            if (user.SalonId == null)
            {
                EmployeeAppealSalons = await _context.EmployeeAppealSalons
                    .Include(r => r.Salon)
                        .ThenInclude(r => r.City)
                    .Where(r => r.EmployeeId == user.Id).ToListAsync();

                ViewData["Salons"] = new SelectList(_context.Salons
                    .Include(r => r.City)
                    .Where(r => !EmployeeAppealSalons.Select(r => r.SalonId).Contains(r.Id))
                    .Select(r => new { r.Id, FullName = r.Name + " (" + r.Address + ")", r.City.Name }),
                    nameof(Models.Salon.Id), nameof(Models.Salon.FullName), null, nameof(Models.Salon.City.Name));
            }
            else
            {
                ViewData["Salon"] = _context.Salons.Where(r => r.Id == user.SalonId).Select(r=> r.Name + " (" + r.Address + ")").FirstOrDefault().ToString();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostInsertAsync()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Error: Tokio salono nėra.";
                return RedirectToPage();
            }
            var user = await _userManager.GetUserAsync(User);
            EmployeeAppealSalon.EmployeeId = user.Id;
            EmployeeAppealSalon.CreatedDate = DateTime.Now;
            _context.EmployeeAppealSalons.Add(EmployeeAppealSalon);

            await _context.SaveChangesAsync();

            StatusMessage = "Prašymas pateiktas salono administratoriams.";

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("Prašymas nerastas");
            }
            user = await _userManager.GetUserAsync(User);
            EmployeeAppealSalon = await _context.EmployeeAppealSalons.Where(r => r.EmployeeId == user.Id && r.Id==id).FirstOrDefaultAsync();

            if (EmployeeAppealSalon != null)
            {
                _context.EmployeeAppealSalons.Remove(EmployeeAppealSalon);
                await _context.SaveChangesAsync();
            }

            StatusMessage = "Jūsų prašymas buvo pašalintas.";
            return RedirectToPage();
        }

    }
}
