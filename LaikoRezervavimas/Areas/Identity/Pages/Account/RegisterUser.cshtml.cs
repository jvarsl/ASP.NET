using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using SalonWithRazor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace SalonWithRazor.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public enum UserRoleEnum { Client = 1, Employee = 2 }

        [BindProperty]
        [Required]
        [EnumDataType(typeof(UserRoleEnum), ErrorMessage = "Pasirinkite elementą iš sąrašo.")]
        //[Display(Name = "Pasirinkite vartotojo vaidmenį")]
        public UserRoleEnum UserRoleChoose { get; set; }
        public class InputModel
        {

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Vardas")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Pavardė")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "El. Paštas (naudojamas prisijungimui)")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} turi būti bent {2} ir ne ilgesnis kaip {1} simbolių.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Slaptažodis")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Patvirtinti slaptažodį")]
            [Compare("Password", ErrorMessage = "Slaptažodis ir patvirtinimo slaptažodis nesutampa.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Lytis")]
            public UserGender UserGender { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null, bool isClient = false, bool isEmployee = false)
        {
            if (isClient)
            {
                UserRoleChoose = (UserRoleEnum)1;
            }
            else if (isEmployee)
            {
                UserRoleChoose = (UserRoleEnum)2;
            }
            else
            {
                return RedirectToPage("Register");
            }
            ViewData["UserType"] = UserRoleChoose == (UserRoleEnum)1 ? "klientui" : "darbuotojui";
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                AppUser user;
                if (UserRoleEnum.Employee == UserRoleChoose)
                {
                    user = new Employee
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        CreatedOn = DateTime.Now,
                        EmployeeSchedule = new List<EmployeeSchedule>()
                        {
                            new EmployeeSchedule
                            {
                                Day = 1,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0),
                                BreakStartTime = new TimeSpan(0, 0, 0),
                                BreakEndTime = new TimeSpan(0, 0, 0)
                            },
                            new EmployeeSchedule
                            {
                                Day = 2,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0),
                                BreakStartTime = new TimeSpan(0, 0, 0),
                                BreakEndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new EmployeeSchedule
                            {
                                Day = 3,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0),
                                BreakStartTime = new TimeSpan(0, 0, 0),
                                BreakEndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new EmployeeSchedule
                            {
                                Day = 4,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0),
                                BreakStartTime = new TimeSpan(0, 0, 0),
                                BreakEndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new EmployeeSchedule
                            {
                                Day = 5,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0),
                                BreakStartTime = new TimeSpan(0, 0, 0),
                                BreakEndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new EmployeeSchedule
                            {
                                Day = 6,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0),
                                BreakStartTime = new TimeSpan(0, 0, 0),
                                BreakEndTime = new TimeSpan(0, 0, 0)
                            }
                            ,
                            new EmployeeSchedule
                            {
                                Day = 7,
                                StartTime = new TimeSpan(0, 0, 0),
                                EndTime = new TimeSpan(0, 0, 0),
                                BreakStartTime = new TimeSpan(0, 0, 0),
                                BreakEndTime = new TimeSpan(0, 0, 0)
                            }
                        }
                    };
                }
                else if (UserRoleEnum.Client == UserRoleChoose)
                {
                    user = new Client
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        CreatedOn = DateTime.Now,
                        Gender = Input.UserGender
                    };
                }
                else
                {
                    return NotFound("Rolė nerasta, paskyros neišėjo sukurti.");
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (user is Client)
                    {
                        await _userManager.AddToRoleAsync(user, "Client");
                    }
                    if (user is Employee)
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // return RedirectToPage("/Index");
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Patvirtinkite savo el. paštą",
                        $"Patvirtinkite savo paskyrą <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>spustelėdami čia</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
