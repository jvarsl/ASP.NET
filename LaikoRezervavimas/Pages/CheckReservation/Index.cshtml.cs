using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor
{
    public class IndexModelForEmployee : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public IndexModelForEmployee(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Reservation> Reservation { get; set; }


        public async Task OnGetAsync()
        {
            ViewData["StatusId"] = new SelectList(_context.ReservationStatuses, "Id", "Description");
            var user = await _userManager.GetUserAsync(User);
            Reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Employee)
                .Include(r => r.Status)
                .Include(r => r.ServiceReservation)
                    .ThenInclude(r => r.Service)
                .Include(r => r.ReservationComment)
                .Where(r => r.EmployeeId == user.Id)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
            ViewData["ReservationStatuses"] = new SelectList(_context.ReservationStatuses, "Description", "Description");
        }
    }
}
