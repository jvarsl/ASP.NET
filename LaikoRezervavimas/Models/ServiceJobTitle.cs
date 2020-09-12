using System;
using System.Collections.Generic;

namespace SalonWithRazor.Models
{
    public partial class ServiceJobTitle
    {
        public int Id { get; set; }
        public int JobTitleId { get; set; }
        public int ServiceId { get; set; }

        public virtual JobTitle JobTitle { get; set; }
        public virtual Service Service { get; set; }
        
    }
}
