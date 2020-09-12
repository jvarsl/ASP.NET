using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public enum NotiflicationType { ReservationStatus, ReservationComment, Favourite }
    public class Notification
    {
        public Notification() { }
        public Notification(int appUserId, string link, NotiflicationType type, int statusId = 0, string commentLeftByUser = "")
        {
            AppUserId = appUserId;

            if ((type == (NotiflicationType)0 && statusId != 0) && (statusId == 2 || statusId== 4))
            {
                var statusDescription = statusId switch
                {
                    2 => "Patvirtinta",
                    4 => "Atšaukta",
                    _ => "",
                };
                Content = $"Rezervacijos būsena buvo pakeista į „{statusDescription.ToLower()}“.";
            }
            if (type == (NotiflicationType)1)
            {
                Content = $"{commentLeftByUser} paliko komentarą prie rezervacijos.";
            }
            if (type == (NotiflicationType)2)
            {
                Content = "Įsiminta paslauga.";
            }

            Link = link;
            Type = type;

            if (type == (NotiflicationType)0)
            {
                IsRead = false;
            }
            if (type == (NotiflicationType)1)
            {
                IsRead = false;
            }
            if (type == (NotiflicationType)2)
            {
                IsRead = true;
            }
        }

        public int Id { get; set; }
        public int AppUserId { get; set; }
        [Display(Name = "Turinys")]
        [MaxLength(500)]
        public string Content { get; set; }
        [Display(Name = "Puslapis")]
        [MaxLength(200)]
        public string Link { get; set; }
        [Display(Name = "Perskaityta")]
        public bool IsRead { get; set; }
        [Required]
        [Display(Name = "Sukurta")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Tipas")]
        public NotiflicationType Type { get; set; }
        public AppUser AppUser { get; set; }

    }
}
