using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public class SalonSchedule
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        [Display(Name = "Darbo diena")]
        public byte? Day { get; set; }
        [Display(Name = "Darbo pradžia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? StartTime { get; set; }
        [Display(Name = "Darbo pabaiga")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? EndTime { get; set; }
        [Display(Name = "Dirba")]
        public bool IsWorking { get; set; }
        public Salon Salon { get; set; }
    }
}
