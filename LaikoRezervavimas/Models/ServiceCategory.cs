using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        [Display(Name = "Kategorija")]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
