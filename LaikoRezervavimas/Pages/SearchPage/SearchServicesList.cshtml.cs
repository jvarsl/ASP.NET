using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor
{
    public class SearchServicesListModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        public SearchServicesListModel(SalonWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string CitySort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int CurrentFilterCategory { get; set; }
        public int CurrentFilterCity { get; set; }
        public PaginatedList<Service> Service { get; set; }

        public async Task OnGetAsync(string sortOrder,
    string currentFilter, int currentFilterCategory, int currentFilterCity, string searchString, int searchCategoryInt, int searchCityInt, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CitySort = sortOrder == "City" ? "city_desc" : "City";
            if (searchString != null || !Tools.Comparer.IsDefaultValue(searchCategoryInt) || !Tools.Comparer.IsDefaultValue(searchCityInt))
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
                searchCategoryInt = currentFilterCategory;
                searchCityInt = currentFilterCity;
            }
            CurrentFilterCity = searchCityInt;
            CurrentFilterCategory = searchCategoryInt;
            CurrentFilter = searchString;

            ViewData["Services"] = new SelectList(_context.Services, nameof(Models.Service.Id), nameof(Models.Service.Name), CurrentFilter);
            ViewData["Cities"] = new SelectList(_context.Cities.Where(r => r.Salon.Any()).OrderBy(r => r.Id), nameof(City.Id), nameof(City.Name), CurrentFilterCity);
            ViewData["Categories"] = new SelectList(_context.ServiceCategory, nameof(ServiceCategory.Id), nameof(ServiceCategory.Name), CurrentFilterCategory);

            IQueryable<Service> serviceIQ = _context.Services
                .Include(e => e.ServiceCategory)
                .Include(e => e.Salon)
                    .ThenInclude(r => r.City)
                .Include(e => e.Salon)
                    .ThenInclude(e => e.Employee)
                .Include(r=>r.ServiceJobTitle)
                    .ThenInclude(r=>r.JobTitle)
                        .ThenInclude(r=>r.Employee)
                .Include(e => e.Salon)
                    .ThenInclude(r => r.Company)
                .Where(r => r.Active && r.Salon.Id != 0 && r.Salon.Employee.Any() && r.ServiceJobTitle.Select(e=>e.JobTitle).Select(e=>e.Employee).Any());


            if (!String.IsNullOrEmpty(searchString))
            {
                serviceIQ = serviceIQ.Where(s => s.Name.Contains(searchString)
                    || s.Salon.Company.Name.Contains(searchString));
            }
            if (!Tools.Comparer.IsDefaultValue(searchCategoryInt))
            {
                serviceIQ = serviceIQ.Where(s => s.ServiceCategoryId == searchCategoryInt);
            }
            if (!Tools.Comparer.IsDefaultValue(searchCityInt))
            {
                serviceIQ = serviceIQ.Where(s => s.Salon.CityId == searchCityInt);
            }

            serviceIQ = sortOrder switch
            {
                "name_desc" => serviceIQ.OrderByDescending(s => s.Name),
                "City" => serviceIQ.OrderBy(s => s.Salon.City.Name),
                "city_desc" => serviceIQ.OrderByDescending(s => s.Salon.City.Name),
                _ => serviceIQ.OrderBy(s => s.Name),
            };

            int pageSize = 20;
            Service = await PaginatedList<Service>.CreateAsync(
                serviceIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
