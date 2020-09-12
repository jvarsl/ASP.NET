using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonWithRazor.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            ReservationComment = new HashSet<ReservationComment>();
            ServiceReservation = new HashSet<ServiceReservation>();
        }

        public int Id { get; set; }
        [Display(Name = "Klientas")]
        public int ClientId { get; set; }
        [Display(Name = "Darbuotojas")]
        public int EmployeeId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [Display(Name = "Pradžia")]
        [Required]
        public DateTime Start { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [Display(Name = "Pabaiga")]
        [Required]
        public DateTime End { get; set; }
        [Required]
        [Display(Name = "Sukurta")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modifikuota")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "Modifikavo")]
        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Klientas")]
        public virtual Client Client { get; set; }
        [Display(Name = "Darbuotojas")]
        public virtual Employee Employee { get; set; }
        [Display(Name = "Būsena")]
        public virtual ReservationStatus Status { get; set; }
        [Display(Name = "Komentarai")]
        public virtual ICollection<ReservationComment> ReservationComment { get; set; }
        [Display(Name = "Paslauga")]
        public virtual ICollection<ServiceReservation> ServiceReservation { get; set; }


    }
}
