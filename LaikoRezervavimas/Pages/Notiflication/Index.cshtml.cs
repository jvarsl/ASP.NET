using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;
using SQLitePCL;

namespace SalonWithRazor.Pages.Notiflication
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
        public class ModelForNotificationIndex
        {
            [Display(Name = "Turinys")]
            public string Content { get; set; }
            [Display(Name = "Adresas")]
            public string Link { get; set; }
            [Display(Name = "Perskaityta")]
            public bool IsRead { get; set; }
            [Required]
            [Display(Name = "Sukurta")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
            public DateTime CreatedDate { get; set; }
            [Display(Name = "Tipas")]
            public NotiflicationType Type { get; set; }
        }
        public IList<ModelForNotificationIndex> ModelForNotificationIndexes { get; set; }

        public int CurrentFilter { get; set; }
        public PaginatedList<Notification> Notification { get; set; }


        public async Task OnGetAsync(int currentFilter, int searchTypeInt, int? pageIndex)
        {
            if (searchTypeInt != 0)
            {
                pageIndex = 1;
            }
            else
            {
                searchTypeInt = currentFilter;
            }

            CurrentFilter = searchTypeInt;

            List<SelectListItem> Types = new List<SelectListItem>();
            Types.Add(new SelectListItem() { Value = "1", Text = "Būsenos" });
            Types.Add(new SelectListItem() { Value = "2", Text = "Komentarai" });
            Types.Add(new SelectListItem() { Value = "3", Text = "Įsimintini" });

            ViewData["Types"] = new SelectList(Types, "Value", "Text", CurrentFilter.ToString());

            var user = await _userManager.GetUserAsync(User);

            IQueryable<Notification> notificationIQ = _context.Notifications
                .Where(r => r.AppUserId == user.Id)
                .OrderByDescending(r => r.CreatedDate);


            if (searchTypeInt != 0)
            {
                var realType = (NotiflicationType) searchTypeInt - 1;
                notificationIQ = notificationIQ.Where(s => s.Type == realType);
            }


            int pageSize = 20;
            Notification = await PaginatedList<Notification>.CreateAsync(
                notificationIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            ModelForNotificationIndexes = Notification
                    .Select(r => new ModelForNotificationIndex()
                    {
                        Content = r.Content,
                        Link = r.Link,
                        IsRead = r.IsRead,
                        CreatedDate = r.CreatedDate,
                        Type = r.Type
                    }).ToList();

            bool isUpdatable = false;
            foreach (var item in Notification.Where(r => r.IsRead == false))
            {
                isUpdatable = true;
                item.IsRead = true;
                _context.Entry(item).State = EntityState.Modified;
            }
            if (isUpdatable)
            {
                _context.SaveChanges();
            }
        }
    }
}
