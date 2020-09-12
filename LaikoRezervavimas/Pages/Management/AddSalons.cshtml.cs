using System;
using System.Collections.Generic;
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
    [Authorize(Roles = "Admin")]
    public class AddSalonsModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AddSalonsModel(
            SalonWithRazor.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IList<Salon> Salons { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", nameof(City.Name));
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", nameof(Company.Name));
            Salons = await _context.Salons
                .Include(s => s.City)
                .Include(s => s.Company).ToListAsync();

            return Page();
        }


        [BindProperty]
        public Salon Salon { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Salon.SalonSchedule = new List<SalonSchedule>()
                        {
                            new SalonSchedule
                            {
                                Day = 1,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0)
                            },
                            new SalonSchedule
                            {
                                Day = 2,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new SalonSchedule
                            {
                                Day = 3,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new SalonSchedule
                            {
                                Day = 4,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new SalonSchedule
                            {
                                Day = 5,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new SalonSchedule
                            {
                                Day = 6,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new SalonSchedule
                            {
                                Day = 7,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0)
                            }
                        };
            _context.Salons.Add(Salon);
            await _context.SaveChangesAsync();

            return RedirectToPage("./AddSalons");
        }

    }
}