using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonWithRazor.Models
{
    public partial class Service
    {
        public Service()
        {
            ServiceJobTitle = new HashSet<ServiceJobTitle>();
            ServiceReservation = new HashSet<ServiceReservation>();
        }
        private string priceString = "";
        private string genderString = "";
        [Display(Name = "Pilnas pavadinimas")]
        public string FullName
        {
            get
            {

                if (!Tools.Comparer.IsDefaultValue(Price))
                {
                    priceString = " - " + Price.ToString() + "€";
                }

                switch (Gender)
                {
                    case UserGender.Vyras:
                        genderString = " (Vyrams)";
                        break;
                    case UserGender.Moteris:
                        genderString = " (Moterims)";
                        break;
                    default:
                        genderString = " (Visiems)";
                        break;

                }
                return string.Concat(Name, priceString, genderString);
            }
        }

        public string EstimatedTimeHumanized
        {
            get
            {
                TimeSpan ts = TimeSpan.FromMinutes(EstimatedTime);
                var hours = (int)ts.TotalHours;
                var minutes = ts.Minutes;
                string estimatedTime = "";
                if (hours != 0)
                {
                    estimatedTime += hours.ToString() + " val";
                }
                if (minutes != 0)
                {
                    if (hours != 0)
                    {
                        estimatedTime += " ";
                    }
                        estimatedTime += minutes.ToString() + " min";
                }
                return estimatedTime;
            }
        }

        public int Id { get; set; }
        [Display(Name = "Paslauga")]
        [StringLength(100)]
        public string Name { get; set; }
        [Display(Name = "Trukmė")]
        [Required]
        [Range(1, 240)]
        public short EstimatedTime { get; set; }
        [Display(Name = "Aprašymas")]
        [MaxLength(500)]
        public string Description { get; set; }
        [Display(Name = "Lytis")]
        public UserGender Gender { get; set; }
        [Display(Name = "Kaina")]
        public int Price { get; set; }
        [Required]
        public bool Active { get; set; }
        [Display(Name = "Kategorija")]
        public int? ServiceCategoryId { get; set; }
        [Required]
        [Display(Name = "Salonas")]
        public int SalonId { get; set; }
        [Display(Name = "Salonas")]
        public virtual Salon Salon { get; set; }
        public virtual ICollection<ServiceJobTitle> ServiceJobTitle { get; set; }
        public virtual ICollection<ServiceReservation> ServiceReservation { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
    }
}
