using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonWithRazor.Models
{
    public partial class Salon
    {
        public Salon()
        {
            Employee = new HashSet<Employee>();
            Service = new HashSet<Service>();
            StaffSalon = new HashSet<StaffSalon>();
            EmployeeAppealSalon = new HashSet<EmployeeAppealSalon>();
        }
        [Display(Name = "Pilnas pavadinimas")]
        public string FullName
        {
            get
            {
                return string.Concat(Name, " (", Address, ")");
            }
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CityId { get; set; }
        [Display(Name = "Adresas")]
        [MaxLength(100)]
        public string Address { get; set; }
        [Display(Name = "Pavadinimas")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Platuma")]
        public double? Latitude { get; set; }
        [Display(Name = "Ilguma")]
        public double? Longitude { get; set; }
        [Display(Name = "Aprašymas")]
        [MaxLength(500)]
        public string Description { get; set; }
        [Display(Name = "Įmonė")]
        public virtual Company Company { get; set; }
        [Display(Name = "Miestas")]
        public virtual City City { get; set; }
        [Display(Name = "Darbuotojai")]
        public virtual ICollection<Employee> Employee { get; set; }
        [Display(Name = "Paslaugos")]
        public virtual ICollection<Service> Service { get; set; }
        public virtual ICollection<StaffSalon> StaffSalon { get; set; }
        public virtual ICollection<EmployeeAppealSalon> EmployeeAppealSalon { get; set; }
        public virtual IList<SalonSchedule> SalonSchedule { get; set; }
    }
}
