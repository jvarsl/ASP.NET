using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonWithRazor.Models
{
    public partial class Company
    {
        public Company()
        {
            Salon = new HashSet<Salon>();
        }

        public int Id { get; set; }
        [Display(Name = "Pavadinimas")]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Salon> Salon { get; set; }
    }
}
