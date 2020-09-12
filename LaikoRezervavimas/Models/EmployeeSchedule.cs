using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonWithRazor.Models
{
    public partial class EmployeeSchedule
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Display(Name = "Darbo diena")]
        public byte? Day { get; set; }
        [Display(Name = "Darbo pradžia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? StartTime { get; set; }
        [Display(Name = "Darbo pabaiga")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? EndTime { get; set; }
        [Display(Name = "Pertraukos pradžia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]

        public TimeSpan? BreakStartTime { get; set; }
        [Display(Name = "Pertraukos pabaiga")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? BreakEndTime { get; set; }
        [Display(Name = "Dirba")]
        public bool IsWorking { get; set; }
        [Display(Name = "Petrauka")]
        public bool IsTakingBreak { get; set; }
        public Employee Employee { get; set; }

    }
}
