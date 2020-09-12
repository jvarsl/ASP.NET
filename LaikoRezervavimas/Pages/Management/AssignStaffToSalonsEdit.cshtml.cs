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
    [Authorize(Roles = "Admin")]
    public class AssignStaffToSalonsEditModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AssignStaffToSalonsEditModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class StaffSalonsVM
        {
            [Display(Name = "Id")]
            public int UserId { get; set; }
            [Display(Name = "Vartotojas")]
            public string UserName { get; set; }
            [Display(Name = "Salono Id")]
            public int SalonId { get; set; }
            [Display(Name = "Salono pavadinimas")]
            public string SalonName { get; set; }
            [Display(Name = "Miestas")]
            public string City { get; set; }
            [Display(Name = "Priklauso personalui")]
            public bool IsSelected { get; set; }
        }
        [BindProperty]
        public IList<StaffSalonsVM> StaffSalons { get; set; }
        public StaffSalon StaffSalon { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }
            var user = await _userManager.FindByIdAsync(id.Value.ToString());
            if (user == null)
            {
                return NotFound("Nerastas toks vartotojas.");
            }

            if (!await _userManager.IsInRoleAsync(user, "Staff"))
            {
                return NotFound();
            }
            StaffSalons = await _context.Salons
                .Include(r => r.StaffSalon.Where(r => r.StaffId == id.Value))
                    .ThenInclude(r => r.Staff)
                .Include(r => r.City)
                .Select(r => new StaffSalonsVM()
                {
                    UserId = user.Id,
                    UserName = user.FullName,
                    SalonId = r.Id,
                    SalonName = r.FullName,
                    City = r.City.Name,
                    IsSelected = r.StaffSalon.Select(e => e.StaffId).Contains(id.Value),
                }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? userId, int? salonId, bool? isSelected)
        {
            if (userId == null || salonId == null || isSelected == null)
            {
                return RedirectToPage("./Index");
            }
            var user = await _userManager.FindByIdAsync(userId.Value.ToString());
            if (user == null)
            {
                return NotFound("Nerastas toks vartotojas.");
            }

            if (!await _userManager.IsInRoleAsync(user, "Staff"))
            {
                return NotFound();
            }

            if (isSelected.Value)
            {
                var staffSalonEntry = await _context.StaffSalons
                    .Where(r => r.StaffId == userId.Value && r.SalonId == salonId.Value).FirstOrDefaultAsync();

                if (staffSalonEntry == null)
                {
                    return NotFound();
                }

                _context.StaffSalons.Remove(staffSalonEntry);
            }
            else if (!isSelected.Value)
            {
                StaffSalon = new StaffSalon();
                StaffSalon.SalonId = salonId.Value;
                StaffSalon.StaffId = userId.Value;
                _context.Add(StaffSalon);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return NotFound("Nepavyko atnaujinti.");
            }
            return RedirectToPage("./AssignStaffToSalonsEdit", new { id = userId.Value });
        }

    }
}