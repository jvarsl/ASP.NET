using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Models;

namespace SalonWithRazor.Areas.Identity.Pages.Account.Manage
{
    public class ChangePictureModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ChangePictureModel(
            SalonWithRazor.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public EmployeePicture EmployeePicture { get; set; }

        [BindProperty]
        public IFormFile UploadPicture { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            EmployeePicture = await _context.EmployeePictures
                .Include(e => e.Employee).FirstOrDefaultAsync(m => m.EmployeeId == user.Id);

            return Page();
        }


        public async Task<IActionResult> OnPostInsertAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid || UploadPicture == null)
            {
                return Page();
            }

            if (Tools.FileImageCheck.IsImage(UploadPicture))
            {

                Image image = Image.FromStream(UploadPicture.OpenReadStream(), true, true);
                var newImage = new Bitmap(200, 200);
                using (var g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(image, 0, 0, 200, 200);
                }
                ImageConverter converter = new ImageConverter();
                EmployeePicture.Picture = (byte[])converter.ConvertTo(newImage, typeof(byte[]));

            }
            else
            {
                StatusMessage = "Error: Įkeltas failas neatpažįstamas kaip nuotrauka.";
                return RedirectToPage();
            }
            EmployeePicture.ModifiedDate = DateTime.Now;
            EmployeePicture.EmployeeId = user.Id;
            if (_context.EmployeePictures.Any(r => r.EmployeeId == user.Id))
            {
                EmployeePicture.Id = await _context.EmployeePictures.Where(r => r.EmployeeId == user.Id).Select(r => r.Id).FirstOrDefaultAsync();
                _context.Attach(EmployeePicture).State = EntityState.Modified;
            }
            else
            {
                _context.EmployeePictures.Add(EmployeePicture);
            }
            await _context.SaveChangesAsync();

            StatusMessage = "Jūsų nuotrauka buvo pakeista.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            EmployeePicture = await _context.EmployeePictures.Where(r => r.EmployeeId == user.Id).FirstOrDefaultAsync();

            if (EmployeePicture != null)
            {
                _context.EmployeePictures.Remove(EmployeePicture);
                await _context.SaveChangesAsync();
            }

            StatusMessage = "Jūsų nuotrauka buvo pašalinta.";
            return RedirectToPage();
        }


    }

}

