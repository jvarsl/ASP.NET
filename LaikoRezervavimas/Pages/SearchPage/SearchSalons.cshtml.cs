using System;
using System.Collections.Generic;
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
    public class SearchSalonPageIndexModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        public SearchSalonPageIndexModel(SalonWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public string CurrentFilter { get; set; }
        public int CurrentFilterCity { get; set; }
        public PaginatedList<Salon> Salon { get; set; }


        public async Task OnGetAsync(string currentFilter, int currentFilterCity, string searchString, int searchCityInt, int? pageIndex)
        {

            if (searchString != null || !Tools.Comparer.IsDefaultValue(searchCityInt))
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
                searchCityInt = currentFilterCity;
            }
            CurrentFilter = searchString;
            CurrentFilterCity = searchCityInt;

            ViewData["Cities"] = new SelectList(_context.Cities.Where(r => r.Salon.Any()), nameof(City.Id), nameof(City.Name), CurrentFilterCity);

            IQueryable<Salon> salonIQ = _context.Salons
                .Include(r => r.City)
                .Include(r => r.Service)
                    .ThenInclude(r => r.ServiceCategory)
                .Include(r => r.SalonSchedule)
                .Where(r => r.Employee.Any());

                                
               // .Where(r => r.Salon.Id == id && r.Active && r.ServiceJobTitle.Select(r => r.JobTitle).Select(r => r.Employee).Any())
            if (!String.IsNullOrEmpty(searchString))
            {
                salonIQ = salonIQ.Where(s => s.Name.Contains(searchString));
            }

            if (!Tools.Comparer.IsDefaultValue(searchCityInt))
            {
                salonIQ = salonIQ.Where(s => s.CityId == searchCityInt);
            }


            int pageSize = 10;
            Salon = await PaginatedList<Salon>.CreateAsync(
                salonIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

        }


    }
}
