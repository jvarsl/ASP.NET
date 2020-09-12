using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonWithRazor.Models
{
    public partial class ReservationComment
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        [Display(Name = "Komentaras")]
        [MinLength(3)]
        [MaxLength(1000)]
        public string Comment { get; set; }
        [Display (Name = "Sukurta")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime PostedDate { get; set; }
        public int? AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Reservation Reservation { get; set; }

    }
}
