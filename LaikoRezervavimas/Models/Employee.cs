using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public class Employee : AppUser
    {
        public Employee()
        {
            EmployeeAppealSalon = new HashSet<EmployeeAppealSalon>();
        }


        [Column(TypeName = "decimal(11, 0)")]
        public decimal NationalId { get; set; }

        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        public int? JobTitleId { get; set; }
        public int? SalonId { get; set; }
        [Display(Name = "Profesija")]

        public virtual JobTitle JobTitle { get; set; }
        [Display(Name = "Salonas")]
        public virtual Salon Salon { get; set; }
        [Display(Name = "Nuotrauka")]
        public virtual EmployeePicture EmployeePicture { get; set; }
        public virtual IList<EmployeeSchedule> EmployeeSchedule { get; set; }
        public virtual ICollection<EmployeeAppealSalon> EmployeeAppealSalon { get; set; }
    }
}
