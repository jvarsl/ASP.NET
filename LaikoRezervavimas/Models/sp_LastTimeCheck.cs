using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public class sp_LastTimeCheck
    {
        public int Id { get; set; }
        public bool Boolean { get; set; }
    }
}
