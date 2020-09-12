using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SalonWithRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;

namespace SalonWithRazor.Pages
{

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public string Referrer { get; set; }
        public bool ShowReferrer => !string.IsNullOrEmpty(Referrer);

        public string ExceptionMessage { get; set; }
        public bool ShowExceptionMessage => !string.IsNullOrEmpty(ExceptionMessage);


        public AppUser AppUser { get; set; } = new AppUser();
        public string userEmail { get; set; }

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }



        public void OnGet()
        {
            if (AppUser.Email != null)
            {
                userEmail = AppUser.Email;
            }
            else { userEmail = ""; }
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            Referrer = HttpContext.Request.Headers["Referer"].ToString();

            // Do NOT expose sensitive error information directly to the client.
            #region snippet_ExceptionHandlerPathFeature
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                ExceptionMessage = "Išmesta failo klaida";
            }
            if (exceptionHandlerPathFeature?.Path == "/Index")
            {
                ExceptionMessage += " iš pagrindinio puslapio";
            }
            #endregion

        }
    }
}
