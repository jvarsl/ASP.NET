using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor
{
    public class SearchEmployeesListModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        public SearchEmployeesListModel(SalonWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string CitySort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<Employee> Employee { get; set; }

        public async Task OnGetAsync(string sortOrder,
    string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CitySort = sortOrder == "City" ? "city_desc" : "City";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
               
            }
            CurrentFilter = searchString;

            IQueryable<Employee> employeeIQ = _context.Employees
                .Include(e => e.JobTitle)
                .Include(e => e.Salon)
                    .ThenInclude(r => r.City)
                .Where(r => r.SalonId != null);
                

            if (!String.IsNullOrEmpty(searchString))
            {
                employeeIQ = employeeIQ.Where(s => s.FirstName.Contains(searchString)
                    || s.LastName.Contains(searchString));
            }

            employeeIQ = sortOrder switch
            {
                "name_desc" => employeeIQ.OrderByDescending(s => s.FirstName),
                "City" => employeeIQ.OrderBy(s => s.Salon.City.Name),
                "city_desc" => employeeIQ.OrderByDescending(s => s.Salon.City.Name),
                _ => employeeIQ.OrderBy(s => s.FirstName),
            };

            int pageSize = 20;
            Employee = await PaginatedList<Employee>.CreateAsync(
                employeeIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
