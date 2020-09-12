using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Admin")]
    public class AddJobTitlesModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AddJobTitlesModel(
            SalonWithRazor.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IList<JobTitle> JobTitles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            JobTitles = await _context.JobTitles.ToListAsync();

            return Page();
        }

        [BindProperty]
        public JobTitle JobTitle { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.JobTitles.Add(JobTitle);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

    }
}