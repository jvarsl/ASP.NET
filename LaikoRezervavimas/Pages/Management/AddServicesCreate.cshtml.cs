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
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Staff")]
    public class AddServicesCreateModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AddServicesCreateModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategory, "Id", "Name");
            ViewData["SalonId"] = new SelectList(_context.Salons
                .Include(r=>r.StaffSalon
                .Where(r=>r.StaffId==user.Id))
                .Where(r=>r.StaffSalon.Select(r=>r.StaffId)
                .Contains(user.Id)), "Id", "Name");
            List<SelectListItem> TimeInMinutes = new List<SelectListItem>();
            for (int i = 15; i<=240; i += 15)
            {
                TimeInMinutes.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }
            ViewData["Times"] = new SelectList(TimeInMinutes.Select(r=> new { Id = r.Value, r.Value }), "Id", "Value");
            var jobTitles = await _context.JobTitles.ToListAsync();

            ServiceJobTitles = new List<ServiceJobTitlesVM>();
            foreach (var item in jobTitles)
            {
                ServiceJobTitles.Add(new ServiceJobTitlesVM { ServiceJobId = item.Id, ServiceJobString = item.Name, IsSelected = false });
            }
            return Page();
        }

        [BindProperty]
        public IList<ServiceJobTitlesVM> ServiceJobTitles { get; set; }

        public class ServiceJobTitlesVM
        {
            [Required]
            public int ServiceJobId { get; set; }
            [Required]
            public string ServiceJobString { get; set; }
            public bool IsSelected { get; set; }
        }
        [BindProperty]
        public Service Service { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
               return await OnGetAsync();
            }
            Service.Active = true;
            foreach (var item in ServiceJobTitles.Where(r => r.IsSelected == true))
            {
                ServiceJobTitle serviceJobTitle = new ServiceJobTitle();
                serviceJobTitle.Service = Service;
                serviceJobTitle.JobTitleId = item.ServiceJobId;
                Service.ServiceJobTitle.Add(serviceJobTitle);
            }
            _context.Services.Add(Service);
            await _context.SaveChangesAsync();

            return RedirectToPage("./AddServices");
        }
    }
}
