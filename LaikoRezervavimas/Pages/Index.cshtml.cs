using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;
        public IndexModel(ILogger<IndexModel> logger,
            SalonWithRazor.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IList<Reservation> Reservation { get; set; }
        public int? EmployeeSalonId { get; private set; }
        public bool IsEmailConfirmed { get; set; } = true;
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Cities"] = new SelectList(_context.Cities.Where(r => r.Salon.Any()).OrderBy(r => r.Id), nameof(City.Id), nameof(City.Name));
            ViewData["ServiceCategories"] = new SelectList(_context.Services.Include(r => r.ServiceCategory).
                Where(r => r.ServiceCategory.Id != 0).Select(r => new { r.ServiceCategory.Id, r.ServiceCategory.Name }).Distinct().OrderBy(r => r.Name),
                nameof(ServiceCategory.Id), nameof(ServiceCategory.Name));
            ViewData["Salons"] = new SelectList(_context.Salons.Where(r => r.Employee.Any()).Distinct().OrderBy(r => r.Name), nameof(Models.Salon.Name), nameof(Models.Salon.Name));
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return Page();
            }
            EmployeeSalonId = await _context.Employees.Where(r => r.Id == user.Id).Select(r => r.SalonId).FirstOrDefaultAsync();

            if (!user.EmailConfirmed)
            {
                IsEmailConfirmed = false;
            }

            if (User.IsInRole("Client"))
            {
                Reservation = await _context.Reservations
                            .Include(r => r.Client)
                            .Include(r => r.Employee)
                                .ThenInclude(r => r.Salon)
                            .Include(r => r.Status)
                            .Include(r => r.ServiceReservation)
                                .ThenInclude(r => r.Service)
                            .Where(r => r.ClientId == user.Id && r.Start > DateTime.Now && r.Status.Id != 3 && r.Status.Id != 4)
                            .OrderBy(r => r.Start)
                            .Take(3)
                            .ToListAsync();
            }
            else if (User.IsInRole("Employee"))
            {
                Reservation = await _context.Reservations
                            .Include(r => r.Client)
                            .Include(r => r.Employee)
                            .Include(r => r.Status)
                            .Include(r => r.ServiceReservation)
                                .ThenInclude(r => r.Service)
                            .Where(r => r.EmployeeId == user.Id && r.Start > DateTime.Now && r.Status.Id != 3 && r.Status.Id != 4)
                            .OrderBy(r => r.Start)
                            .Take(3)
                            .ToListAsync();
            }
            return Page();
        }

    }
}
