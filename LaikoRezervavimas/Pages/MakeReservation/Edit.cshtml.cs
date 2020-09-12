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
    public class EditModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public EditModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Page();
            }

            Reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Employee)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id && user.Id == m.ClientId);

            if (Reservation == null)
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
            if (Reservation.ClientId != user.Id)
            {
                return RedirectToPage("./Index");
            }

            if (Reservation.Status.Id == 4)
            {
                var notification = new Notification(Reservation.EmployeeId, $"/CheckReservation/Details?id={Reservation.Id}", (NotiflicationType)0, Reservation.Status.Id);
                _context.Add(notification);
            }

            Reservation.ModifiedBy = user.FullName;
            Reservation.ModifiedDate = DateTime.Now;

            _context.Attach(Reservation).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(Reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
