using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonWithRazor.Models
{
    public partial class ReservationStatus
    {
        public ReservationStatus()
        {
            Reservation = new HashSet<Reservation>();
        }
        

        public int Id { get; set; }
        [Display(Name = "Būsena")]
        [MaxLength(100)]
        public string Description { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
