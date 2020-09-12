using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonWithRazor.Models
{
    public partial class JobTitle
    {
        public JobTitle()
        {
            Employee = new HashSet<Employee>();
            ServiceJobTitle = new HashSet<ServiceJobTitle>();
        }

        public int Id { get; set; }
        [Display(Name = "Profesija")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<ServiceJobTitle> ServiceJobTitle { get; set; }
    }
}
