using SalonWithRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Interfaces
{
    public interface IMakeReservationCategory
    {
        IEnumerable<City> GetCities();
        IEnumerable<Salon> GetSalons(int cityId);
        IEnumerable<Employee> GetEmployees(int salonId);
        IEnumerable<Service> GetServices(int employeeId);
        IEnumerable<Service> GetServices2(int employeeId, int serviceId1);
        IEnumerable<Service> GetServices3(int employeeId, int serviceId1, int serviceId2);
    }
}
