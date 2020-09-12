using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalonWithRazor.Data;
using SalonWithRazor.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SalonWithRazor.ServiceModels;
using SalonWithRazor.Interfaces;

namespace SalonWithRazor
{
    public class CreateModel : PageModel
    {
        private readonly SalonWithRazor.Data.ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        private readonly IMakeReservationCategory _categoryService;

        public CreateModel(SalonWithRazor.Data.ApplicationDbContext context, UserManager<AppUser> userManager, IMakeReservationCategory categoryService)
        {
            _context = context;
            _userManager = userManager;
            _categoryService = categoryService;
        }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Miestas")]
        public int CityId { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Salonas")]
        public int SalonId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int EmployeeId { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Paslauga (pirma)")]
        public int ServiceId1 { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ServiceId2 { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ServiceId3 { get; set; }

        public IActionResult OnGet()
        {
            ViewData["Cities"] = new SelectList(_categoryService.GetCities(), nameof(City.Id), nameof(City.Name));
            ViewData["Salons"] = new SelectList(_categoryService.GetSalons(0).Select(r => new { r.Id, FullName = r.Name + " (" + r.Address + ")", r.City.Name }), nameof(Models.Salon.Id), nameof(Models.Salon.FullName), null, nameof(Models.Salon.City.Name));
            ViewData["EmployeeId"] = new SelectList(_categoryService.GetEmployees(0), nameof(Employee.Id), nameof(Employee.FullName));
            ViewData["ServiceName"] = new SelectList(_categoryService.GetServices(0), nameof(Service.Id), nameof(Service.FullName));
            ViewData["ServiceName2"] = new SelectList(_categoryService.GetServices2(0, 0), nameof(Service.Id), nameof(Service.FullName));
            ViewData["ServiceName3"] = new SelectList(_categoryService.GetServices3(0, 0, 0), nameof(Service.Id), nameof(Service.FullName));
            return Page();
        }

        public IActionResult OnGetFromSalonPage(int cityId, int salonId, int employeeId, int serviceId1)
        {
            ViewData["Cities"] = new SelectList(_categoryService.GetCities(), nameof(City.Id), nameof(City.Name), cityId);
            ViewData["Salons"] = new SelectList(_categoryService.GetSalons(cityId), nameof(Salon.Id), nameof(Salon.FullName), salonId);
            ViewData["EmployeeId"] = new SelectList(_categoryService.GetEmployees(salonId), nameof(Employee.Id), nameof(Employee.FullName), employeeId);
            ViewData["ServiceName"] = new SelectList(_categoryService.GetServices(employeeId), nameof(Service.Id), nameof(Service.FullName), serviceId1);
            ViewData["ServiceName2"] = new SelectList(_categoryService.GetServices2(employeeId, serviceId1), nameof(Service.Id), nameof(Service.FullName));
            ViewData["ServiceName3"] = new SelectList(_categoryService.GetServices3(0, 0, 0), nameof(Service.Id), nameof(Service.FullName));
            //StartDate = new DateTime(2020, 05, 04).Date;
            return Page();
        }


        public JsonResult OnGetSalons()
        {
            return new JsonResult(_categoryService.GetSalons(CityId).Select(r => new { r.Id, r.FullName }));
        }

        public JsonResult OnGetEmployees()
        {
            return new JsonResult(_categoryService.GetEmployees(SalonId).Select(r => new { r.Id, r.FullName }));
        }
        public JsonResult OnGetServices()
        {
            return new JsonResult(_categoryService.GetServices(EmployeeId).Select(r => new { r.Id, r.FullName }).Distinct());
        }

        public JsonResult OnGetServices2()
        {
            return new JsonResult(_categoryService.GetServices2(EmployeeId, ServiceId1).Select(r => new { r.Id, r.FullName }).Distinct());
        }
        public JsonResult OnGetServices3()
        {
            return new JsonResult(_categoryService.GetServices3(EmployeeId, ServiceId1, ServiceId2).Select(r => new { r.Id, r.FullName }).Distinct());
        }


        [BindProperty]
        public Service Service { get; set; }
        [BindProperty]
        public Service Service2 { get; set; }
        [BindProperty]
        public Service Service3 { get; set; }

        [BindProperty]
        public ServiceReservation ServiceReservation { get; set; }
        [BindProperty]
        public ServiceReservation ServiceReservation2 { get; set; }
        [BindProperty]
        public ServiceReservation ServiceReservation3 { get; set; }

        [BindProperty]
        [Required]
        public Reservation Reservation { get; set; }

        [BindProperty]
        public ReservationComment ReservationComment { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Data")]
        public DateTime? StartDate { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Laikas (pasirinkti iš lentelės)")]
        public TimeSpan? StartTime { get; set; }

        public IList<sp_Hours24Table> sp_Hours24Table { get; set; }

        [BindProperty]
        [Required]
        public PostData PostDataValues { get; set; }

        public class PostData
        {
            [Required]
            public int Service1 { get; set; }
            [Required]
            [DataType(DataType.Date)]
            public DateTime Date { get; set; }
            [Required]
            public int EmployeeId { get; set; }
            public int Service2 { get; set; }
            public int Service3 { get; set; }
        }

        public async Task<IActionResult> OnPostReservationAsync()
        {
            ModelState.Remove("CityId");
            ModelState.Remove("SalonId");
            ModelState.Remove("ServiceId2");
            ModelState.Remove("ServiceId3");
            ModelState.Remove("EstimatedTime");
            if (!ModelState.IsValid || StartTime.Value.Minutes%15!=0)
            {
                ViewData["Cities"] = new SelectList(_categoryService.GetCities(), nameof(City.Id), nameof(City.Name));
                ViewData["Salons"] = new SelectList(_categoryService.GetSalons(CityId).Select(r => new { r.Id, FullName = r.Name + " (" + r.Address + ")", r.City.Name }), nameof(Models.Salon.Id), nameof(Models.Salon.FullName), null, nameof(Models.Salon.City.Name));
                ViewData["EmployeeId"] = new SelectList(_categoryService.GetEmployees(SalonId), nameof(Employee.Id), nameof(Employee.FullName));
                ViewData["ServiceName"] = new SelectList(_categoryService.GetServices(Reservation.EmployeeId), nameof(Service.Id), nameof(Service.FullName));
                ViewData["ServiceName2"] = new SelectList(_categoryService.GetServices2(Reservation.EmployeeId, ServiceId1), nameof(Service.Id), nameof(Service.FullName));
                ViewData["ServiceName3"] = new SelectList(_categoryService.GetServices3(Reservation.EmployeeId, ServiceId1, ServiceId2), nameof(Service.Id), nameof(Service.FullName));
                return Page();
            }
            ServiceReservation.ServiceId = ServiceId1;
            Service = await _context.Services.FindAsync(ServiceReservation.ServiceId);
            if (!Tools.Comparer.IsDefaultValue(ServiceId2))
            {
                ServiceReservation2.ServiceId = ServiceId2;
                Service2 = await _context.Services.FindAsync(ServiceReservation2.ServiceId);
            }
            if (!Tools.Comparer.IsDefaultValue(ServiceId3))
            {
                ServiceReservation3.ServiceId = ServiceId3;
                Service3 = await _context.Services.FindAsync(ServiceReservation3.ServiceId);
            }
            Reservation.Start = StartDate.Value.Date + StartTime.Value;
            Reservation.End = Reservation.Start.AddMinutes(Service.EstimatedTime + Service2.EstimatedTime + Service3.EstimatedTime);
            var user = await _userManager.GetUserAsync(User);
            Reservation.ClientId = user.Id;
            TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;
            Reservation.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemTimeZone); // DateTime.UtcNow;
            Reservation.Status = _context.ReservationStatuses.Find(1);

            if (ReservationComment.Comment != null)
            {
                ReservationComment.PostedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemTimeZone);
                ReservationComment.AppUserId = user.Id;
                Reservation.ReservationComment.Add(ReservationComment);
            }


            Reservation.ServiceReservation.Add(ServiceReservation);

            if (!Tools.Comparer.IsDefaultValue(ServiceReservation2.ServiceId)) Reservation.ServiceReservation.Add(ServiceReservation2);
            if (!Tools.Comparer.IsDefaultValue(ServiceReservation3.ServiceId)) Reservation.ServiceReservation.Add(ServiceReservation3);

            //verify that time wasn't taken while client was picking times
            var listOfServiceId = new List<int> { ServiceId1, ServiceId2, ServiceId3 };
            int serviceEstimatedTime =  _context.Services.Where(r => listOfServiceId.Contains(r.Id)).Sum(r => r.EstimatedTime);
            IList<sp_LastTimeCheck> ssp_LastTimeCheck = await _context.sp_LastTimeChecks.FromSqlRaw("EXECUTE dbo.LastVerifyEmployeeAvailableTime " +
                     "@EmployeeId = {0}, @ServiceDate = {1}, @ServiceEstimatedTime = {2}, @ProvidedTime = {3}",
                        Reservation.EmployeeId, 
                        StartDate.Value.Date,
                        serviceEstimatedTime,
                        StartTime.Value).ToListAsync();

            bool isAvailable = ssp_LastTimeCheck.Where(r => r.Id == 1).Select(r => r.Boolean).First();

            if (isAvailable && Reservation.Start> TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemTimeZone))
            {
                _context.Add(Reservation);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            else
            {
                ViewData["Cities"] = new SelectList(_categoryService.GetCities(), nameof(City.Id), nameof(City.Name));
                ViewData["Salons"] = new SelectList(_categoryService.GetSalons(CityId), nameof(Models.Salon.Id), nameof(Models.Salon.FullName), null, nameof(Models.Salon.City.Name));
                ViewData["EmployeeId"] = new SelectList(_categoryService.GetEmployees(SalonId), nameof(Employee.Id), nameof(Employee.FullName));
                ViewData["ServiceName"] = new SelectList(_categoryService.GetServices(Reservation.EmployeeId), nameof(Service.Id), nameof(Service.FullName));
                ViewData["ServiceName2"] = new SelectList(_categoryService.GetServices2(Reservation.EmployeeId, ServiceId1), nameof(Service.Id), nameof(Service.FullName));
                ViewData["ServiceName3"] = new SelectList(_categoryService.GetServices3(Reservation.EmployeeId, ServiceId1, ServiceId2), nameof(Service.Id), nameof(Service.FullName));
                return Page();
            }
        }

        public async Task<IActionResult> OnPostSendAsync()
        {
            int service1 = PostDataValues.Service1;
            DateTime serviceDate = PostDataValues.Date;
            int serviceEmployeeId = PostDataValues.EmployeeId;

            int service2 = PostDataValues.Service2;
            int service3 = PostDataValues.Service3;
            {
                MemoryStream stream = new MemoryStream();
                Request.Body.CopyTo(stream);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string requestBody = reader.ReadToEnd();
                    if (requestBody.Length > 0)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                        };
                        try
                        {
                            var obj = JsonConvert.DeserializeObject<PostData>(requestBody, settings);
                            if (obj != null && !Tools.Comparer.IsDefaultValue(obj.Service1)
                                && !Tools.Comparer.IsDefaultValue(obj.Date)
                                && !Tools.Comparer.IsDefaultValue(obj.EmployeeId))
                            {
                                service1 = obj.Service1;
                                serviceDate = obj.Date;
                                serviceEmployeeId = obj.EmployeeId;
                                service2 = obj.Service2;
                                service3 = obj.Service3;
                            }
                            else { throw new Exception("Tuščios vertės buvo perduotos"); }
                        }
                        catch (Exception) { return BadRequest("Blogai pateikti duomenys"); }
                    }

                }
            }
            var listOfServiceId = new List<int> { service1, service2, service3 };
            int serviceEstimatedTime = await _context.Services.Where(r => listOfServiceId.Contains(r.Id)).SumAsync(r => r.EstimatedTime);

            
            IList<sp_Hours24Table> ssp_Hours24Table = await _context.sp_Hours24Tables.FromSqlRaw("EXECUTE dbo.GetEmployeeAvailableTime " +
                "@EmployeeId = {0}, @ServiceDate = {1}, @ServiceEstimatedTime = {2}",
                serviceEmployeeId, serviceDate, serviceEstimatedTime).ToListAsync();
                

            if (Content(JsonConvert.SerializeObject(ssp_Hours24Table), "application/json").Content == "[]")
            {
                return BadRequest("Nėra laisvo laiko");
            }

            return Content(JsonConvert.SerializeObject(ssp_Hours24Table), "application/json");
        }


    }
}
