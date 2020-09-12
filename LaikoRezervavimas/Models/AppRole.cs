using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public class AppRole : IdentityRole<int>
    {
        [Display(Name="Rolė")]
        [MaxLength(50)]
        public string LithuanianName { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
