using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.SalonPage
{
    public class IndexModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public int CurrentFilterCategory { get; set; }
        public int Id { get; set; }
        public Salon Salon { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id, int serviceCategoryId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Salon = await _context.Salons
                .Include(r => r.City)
                .Include(r => r.Company)
                .Include(r => r.Employee)
                    .ThenInclude(r => r.JobTitle)
                    .ThenInclude(r => r.ServiceJobTitle)
                    .ThenInclude(r => r.Service)
                    .ThenInclude(r => r.ServiceCategory)
                .Include(r => r.Employee)
                    .ThenInclude(r => r.EmployeePicture)
                .Include(r => r.Employee)
                    .ThenInclude(r => r.EmployeeSchedule)
                .Include(r => r.SalonSchedule)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (Salon == null)
            {
                return NotFound();
            }
            CurrentFilterCategory = serviceCategoryId;

            ViewData["ServiceCategories"] = new SelectList(_context.Services
                    .Include(r => r.ServiceCategory)
                    .Include(r => r.Salon)
                    .Include(r=>r.ServiceJobTitle)
                        .ThenInclude(r=>r.JobTitle)
                             .ThenInclude(r=>r.Employee)
                .Where(r => r.Salon.Id == id && r.Active && r.ServiceJobTitle.Select(r=>r.JobTitle).Select(r=>r.Employee).Any())
                .Where(r => r.ServiceCategoryId != 0)
                .Select(r => new { r.ServiceCategory.Id, r.ServiceCategory.Name })
                .Distinct()
                .OrderBy(r => r.Name),
                nameof(ServiceCategory.Id), nameof(ServiceCategory.Name), CurrentFilterCategory);


            return Page();
        }

        public class PostData
        {
            [Required]
            public int UserId { get; set; }
            [Required]
            public string Link { get; set; }
        }
        [BindProperty]
        [Required]
        public PostData PostDataValues { get; set; }
        public async Task<IActionResult> OnPostCreateNotificationAsync()
        {
            int userId = PostDataValues.UserId;
            string link = PostDataValues.Link;
            {
                MemoryStream stream = new MemoryStream();
                Request.Body.CopyTo(stream);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string requestBody = reader.ReadToEnd();
                    if (requestBody.Length > 0)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                        };
                        try
                        {
                            var obj = JsonConvert.DeserializeObject<PostData>(requestBody, settings);
                            if (obj != null && !Tools.Comparer.IsDefaultValue(obj.UserId)
                                && !Tools.Comparer.IsDefaultValue(obj.Link))
                            {
                                userId = obj.UserId;
                                link = obj.Link;

                                var notification = new Notification(userId, link, (NotiflicationType)2);
                                _context.Add(notification);
                                await _context.SaveChangesAsync();
                            }
                            else { throw new Exception("Tuščios vertės buvo perduotos"); }
                        }
                        catch (Exception e) { return BadRequest(e); }
                    }

                }
                return new OkResult();

            }


        }


    }
}
