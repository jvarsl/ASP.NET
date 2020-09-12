using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public class EmployeeAppealSalon
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int SalonId { get; set; }
        [Required]
        [Display(Name = "Užklausimo laikas")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
        public virtual Employee Employee { get; set; }
        [Display(Name = "Salonas")]
        public virtual Salon Salon { get; set; }
    }
}
