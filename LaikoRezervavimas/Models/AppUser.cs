using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public enum UserGender { Vyras, Moteris, Visi }
    public class AppUser : IdentityUser<int>
    {
        
        public AppUser()
        {
            Reservation = new HashSet<Reservation>();
            StaffSalon = new HashSet<StaffSalon>();
            Notification = new HashSet<Notification>();
            AppUserRole = new HashSet<AppUserRole>();
        }

        [Display(Name = "Vardas")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", UppercaseFirst(FirstName), UppercaseFirst(LastName));
            }
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        [Display(Name = "Vardas")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Pavardė")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Display(Name = "Lytis")]
        public UserGender Gender { get; set; } = (UserGender)2;
        public DateTime? CreatedOn { get; set; }
        [Display(Name = "Užblokuotas")]
        public bool Blocked { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
        public virtual ICollection<StaffSalon> StaffSalon { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<AppUserRole> AppUserRole { get; set; }
    }
}
