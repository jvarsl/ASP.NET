using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Data;
using SalonWithRazor.Models;

namespace SalonWithRazor
{
    public class SearchPageListModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        public SearchPageListModel(SalonWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Salon> Salon { get;set; }

        public async Task OnGetAsync()
        {
            Salon = await _context.Salons
                .Include(s => s.City)
                .Include(s => s.Company).ToListAsync();
        }
    }
}
