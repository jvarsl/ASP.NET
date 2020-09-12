using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonWithRazor.Models
{
    public partial class ServiceReservation
    {
        public int Id { get; set; }
        [Display(Name = "Rezervacija")]
        public int ReservationId { get; set; }
        [Display(Name = "Paslauga")]
        public int ServiceId { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual Service Service { get; set; }
    }
}
