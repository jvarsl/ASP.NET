using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalonWithRazor.Migrations;
using SalonWithRazor.Models;

namespace SalonWithRazor.Pages.Management
{
    [Authorize(Roles = "Admin")]
    public class AssignStaffEmployeesEditModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AssignStaffEmployeesEditModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class UserRolesVM
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public bool IsSelected { get; set; }
        }
        [BindProperty]
        public UserRolesVM UserRole { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }
            var user = await _userManager.FindByIdAsync(id.Value.ToString());
            if (user == null)
            {
                return NotFound("Nerastas toks vartotojas.");
            }
            UserRole = new UserRolesVM();
            var role = await _roleManager.FindByIdAsync("3");
            UserRole.RoleName = role.Name;
            UserRole.UserName = user.FullName;
            UserRole.UserId = user.Id;
            UserRole.RoleId = role.Id;
            if (await _userManager.IsInRoleAsync(user, UserRole.RoleName))
            {
                UserRole.IsSelected = true;
            }
            else
            {
                UserRole.IsSelected = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }
            var user = await _userManager.FindByIdAsync(id.Value.ToString());
            if (user == null)
            {
                return NotFound("Nerastas toks vartotojas.");
            }
            var role = await _roleManager.FindByIdAsync("3");

            IdentityResult result;
            if (!UserRole.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
            {
                result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    return NotFound("Nepavyko nuimti rolės.");
                }
            }

            if (UserRole.IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
            {
                result = await _userManager.AddToRoleAsync(user, UserRole.RoleName);
                if (!result.Succeeded)
                {
                    return NotFound("Nepavyko pridėti rolės.");
                }
            }

            return RedirectToPage("./AssignStaffEmployees");
        }

    }
}