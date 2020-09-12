using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public class StaffSalon
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int SalonId { get; set; }
        public virtual AppUser Staff { get; set; }
        public virtual Salon Salon { get; set; }
    }
}
