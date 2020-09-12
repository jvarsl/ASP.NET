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
    public class AddServicesEditModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AddServicesEditModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public Service Service { get; set; }

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

            Service = await _context.Services
                .Include(s => s.ServiceCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (Service == null || !staffManagesSalonIds.Contains(Service.SalonId))
            {
                return NotFound();
            }

            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategory, "Id", "Name");
            List<SelectListItem> TimeInMinutes = new List<SelectListItem>();
            for (int i = 15; i <= 240; i += 15)
            {
                TimeInMinutes.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }
            ViewData["Times"] = new SelectList(TimeInMinutes.Select(r => new { Id = r.Value, r.Value }), "Id", "Value");
            var jobTitles = await _context.JobTitles.ToListAsync();

            ServiceJobTitles = new List<ServiceJobTitlesVM>();
            var belongsToJobTitles = await _context.ServiceJobTitles.Where(r => r.ServiceId == Service.Id).Select(r => r.JobTitleId).ToListAsync();
            foreach (var item in jobTitles)
            {
                ServiceJobTitles.Add(new ServiceJobTitlesVM { ServiceJobId = item.Id, ServiceJobString = item.Name, IsSelected = belongsToJobTitles.Contains(item.Id) });
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
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategory, "Id", "Name");
                List<SelectListItem> TimeInMinutes = new List<SelectListItem>();
                for (int i = 15; i <= 240; i += 15)
                {
                    TimeInMinutes.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                }
                ViewData["Times"] = new SelectList(TimeInMinutes.Select(r => new { Id = r.Value, r.Value }), "Id", "Value");
                return Page();
            }

            var belongsToJobTitles = await _context.ServiceJobTitles.Where(r => r.ServiceId == Service.Id).Select(r => r.JobTitleId).ToListAsync();

            foreach (var item in ServiceJobTitles)
            {
                ServiceJobTitle serviceJobTitle = new ServiceJobTitle();
                if (item.IsSelected)
                {
                    if (belongsToJobTitles.Contains(item.ServiceJobId))
                    {
                        //the item was already there
                    }
                    else
                    {
                        serviceJobTitle.Service = Service;
                        serviceJobTitle.JobTitleId = item.ServiceJobId;
                        Service.ServiceJobTitle.Add(serviceJobTitle);
                    }
                }
                else if (!item.IsSelected)
                {
                    if (belongsToJobTitles.Contains(item.ServiceJobId))
                    {
                        serviceJobTitle = await _context.ServiceJobTitles.Where(r => r.ServiceId == Service.Id && r.JobTitleId == item.ServiceJobId).FirstOrDefaultAsync();
                        if (serviceJobTitle != null)
                        {
                            _context.ServiceJobTitles.Remove(serviceJobTitle);
                        }
                    }
                    else
                    {
                        //item wasn't there
                    }
                }
            }
            _context.Attach(Service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(Service.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./AddServices");
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
