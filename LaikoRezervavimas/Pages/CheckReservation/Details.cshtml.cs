using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor
{
    public class DetailsModelForEmployee : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DetailsModelForEmployee(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Reservation Reservation { get; set; }
        public ReservationComment ReservationComment { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            Reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Employee)
                    .ThenInclude(r => r.Salon)
                    .ThenInclude(r => r.City)
                .Include(r => r.Status)
                .Include(r => r.ReservationComment)
                .Include(r => r.ServiceReservation)
                    .ThenInclude(r => r.Service)
                    .Where(r => r.EmployeeId == user.Id)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Reservation == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAddCommentAsync(ReservationComment newComment)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Page();
            }

            newComment.AppUserId = user.Id;
            newComment.PostedDate = DateTime.Now;
            _context.Add(newComment);

            var reservation = await _context.Reservations.Where(r => r.Id == newComment.ReservationId).FirstOrDefaultAsync();
            var notification = new Notification(reservation.ClientId, $"/MakeReservation/Details?id={reservation.Id}", (NotiflicationType)1, 0, user.FullName);
            _context.Add(notification);


            var successful = await _context.SaveChangesAsync();
            if (successful < 0)
            {
                return BadRequest("Nepavyko pridėti komentaro.");
            }

            return Redirect($"/CheckReservation/Details?id={newComment.ReservationId}");
        }
    }
}
