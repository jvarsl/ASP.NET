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
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.SearchPage
{
    public class SearchServicePageIndexModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        public SearchServicePageIndexModel(SalonWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public int CurrentFilterCategory { get; set; }
        public int CurrentFilterCity { get; set; }
        [Display(Name = "Data")]
        public DateTime? CurrentFilterDate { get; set; }
        [Display(Name = "Paslaugos pavadinimas")]
        public string CurrentFilterServiceName { get; set; }
        public PaginatedList<Salon> Salon { get; set; }


        public async Task OnGetAsync(int currentFilterCategory, int currentFilterCity, DateTime currentFilterDate, string currentFilterServiceName, int searchServiceCategory, int searchCityInt, DateTime searchDateDT, string searchServiceName, int? pageIndex)
        {

            if (!Tools.Comparer.IsDefaultValue(searchServiceCategory) || !Tools.Comparer.IsDefaultValue(searchCityInt) || !Tools.Comparer.IsDefaultValue(searchDateDT) || !String.IsNullOrEmpty(searchServiceName))
            {
                pageIndex = 1;
            }
            else
            {
                searchServiceCategory = currentFilterCategory;
                searchCityInt = currentFilterCity;
                searchDateDT = currentFilterDate;
                searchServiceName = currentFilterServiceName;
            }
            CurrentFilterCategory = searchServiceCategory;
            CurrentFilterCity = searchCityInt;
            CurrentFilterDate = searchDateDT;
            CurrentFilterServiceName = searchServiceName;

            ViewData["Cities"] = new SelectList(_context.Cities.Where(r => r.Salon.Any()).OrderBy(r => r.Id), nameof(City.Id), nameof(City.Name), CurrentFilterCity);
            ViewData["ServiceCategories"] = new SelectList(_context.Services.Include(r => r.ServiceCategory).
                Where(r => r.ServiceCategory.Id != 0).Select(r => new { r.ServiceCategory.Id, r.ServiceCategory.Name }).Distinct().OrderBy(r => r.Name),
                nameof(ServiceCategory.Id), nameof(ServiceCategory.Name), CurrentFilterCategory);

            IQueryable<Salon> salonIQ = _context.Salons
                    .Include(r => r.City)
                    .Include(r => r.Service
                        .Where(e => e.Active && e.ServiceJobTitle.Select(e => e.JobTitle).Select(e => e.Employee).Any()))
                        .ThenInclude(r => r.ServiceCategory)
                .Include(r => r.Service)
                    .ThenInclude(r => r.ServiceJobTitle)
                        .ThenInclude(r => r.JobTitle)
                             .ThenInclude(r => r.Employee)
                    .Include(r => r.SalonSchedule)
                    .Where(r => r.Employee.Any());



            if (!Tools.Comparer.IsDefaultValue(searchServiceCategory))
            {
                //is in category
                salonIQ = salonIQ.Where(r => r.Service
                    .Where(s => s.ServiceCategoryId == searchServiceCategory)
                    .Select(e => e.ServiceCategoryId)
                    .Contains(searchServiceCategory));
            }

            if (!Tools.Comparer.IsDefaultValue(searchCityInt))
            {
                //is in city
                salonIQ = salonIQ.Where(s => s.CityId == searchCityInt);
            }

            if (!Tools.Comparer.IsDefaultValue(searchDateDT))
            {
                //works during given date week day
                var currentDay = (byte)searchDateDT.DayOfWeek;
                salonIQ = salonIQ.Where(r => r.SalonSchedule.Where(s => s.Day == currentDay && s.StartTime != s.EndTime).Select(e => e.SalonId).Contains(r.Id));
            }

            if (!String.IsNullOrEmpty(searchServiceName))
            {
                //salonIQ = salonIQ.Where(r => r.Service.Select(r => r.Name).Contains(searchServiceName));
            }

            int pageSize = 10;
            Salon = await PaginatedList<Salon>.CreateAsync(
                salonIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }


    }
}
