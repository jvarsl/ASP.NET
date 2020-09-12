using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Interfaces;
using SalonWithRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.ServiceModels
{
    public class MakeReservationCategory : IMakeReservationCategory
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        public MakeReservationCategory(SalonWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities
                .Include(r => r.Salon)
                .Where(r => r.Salon.Any())
                .ToList();
        }
        public IEnumerable<Salon> GetSalons(int cityId = 0)
        {
            if (Tools.Comparer.IsDefaultValue(cityId))
            {
                return _context.Salons.Include(r=>r.City).Include(r=>r.Employee).Where(r=>r.Employee.Any()).ToList();
            }
            else
            {
                return _context.Salons.Include(r => r.City).Include(r => r.Employee).Where(r => r.Employee.Any()).Where(s => s.CityId == cityId).ToList();
            }
        }

        public IEnumerable<Employee> GetEmployees(int salonId = 0)
        {
            if (Tools.Comparer.IsDefaultValue(salonId))
            {
                return _context.Employees.Where(r => salonId != 0).ToList();
            }
            else
            {
                return _context.Employees
                    .Where(r => r.SalonId == salonId)
                    .ToList();
            }

        }

        public IEnumerable<Service> GetServices(int employeeId)
        {
            return (from service in _context.Services
                    join serviceJobTitle in _context.ServiceJobTitles on service.Id equals serviceJobTitle.ServiceId
                    join salon in _context.Salons on service.Salon.Id equals salon.Id
                    join employee in _context.Employees on serviceJobTitle.JobTitleId equals employee.JobTitleId
                    where employee.Id == employeeId && employee.SalonId == salon.Id && service.Active == true
                    select service).ToList();

    /*
             //net core 5 will have where clauses in include
            return _context.Services
                .Include(r => r.ServiceJobTitle)
                .Include(r => r.Salon)
                .ThenInclude(r => r.Employee)
                .Where(r => r.Salon.Employee.Any(e => e.Id == employeeId))
                .ToList();
                */

        }

        public IEnumerable<Service> GetServices2(int employeeId, int serviceId1)
        {
            return (from service in _context.Services
                    join serviceJobTitle in _context.ServiceJobTitles on service.Id equals serviceJobTitle.ServiceId
                    join salon in _context.Salons on service.Salon.Id equals salon.Id
                    join employee in _context.Employees on serviceJobTitle.JobTitleId equals employee.JobTitleId
                    where employee.Id == employeeId && employee.SalonId == salon.Id && service.Id != serviceId1 && service.Active == true
                    select service).ToList();

        }
        public IEnumerable<Service> GetServices3(int employeeId, int serviceId1, int serviceId2)
        {
            return (from service in _context.Services
                    join serviceJobTitle in _context.ServiceJobTitles on service.Id equals serviceJobTitle.ServiceId
                    join salon in _context.Salons on service.Salon.Id equals salon.Id
                    join employee in _context.Employees on serviceJobTitle.JobTitleId equals employee.JobTitleId
                    where employee.Id == employeeId && employee.SalonId == salon.Id && service.Id != serviceId1 && service.Id != serviceId2 && service.Active == true
                    select service).ToList();

        }
    }
}
