using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonWithRazor.Models
{
    public partial class City
    {
        public City()
        {
            Salon = new HashSet<Salon>();
        }
        public int Id { get; set; }
        [Display(Name = "Miestas")]
        [MaxLength(100)]
        public string Name { get; set; }
        public virtual ICollection<Salon> Salon { get; set; }
    }
}
