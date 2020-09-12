using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalonWithRazor.Data;
using SalonWithRazor.Models;
using SalonWithRazor.ServiceModels;
using SalonWithRazor.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Globalization;
using System.Collections.Generic;

namespace SalonWithRazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireClientRole", policy => policy.RequireRole("Client"));
                options.AddPolicy("RequireEmployeeRole", policy => policy.RequireRole("Employee"));
                options.AddPolicy("RequireStaffRole", policy => policy.RequireRole("Staff"));
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireBlockedPageRole", policy => policy.RequireRole("BlockedPage"));
            });

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddRazorPages()
                .AddRazorRuntimeCompilation()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/MakeReservation", "RequireClientRole");
                    options.Conventions.AuthorizeFolder("/CheckReservation", "RequireEmployeeRole");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Manage/EmployeeAppealToSalon", "RequireEmployeeRole");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Manage/TwoFactorAuthentication", "RequireBlockedPageRole");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Manage/PersonalData", "RequireBlockedPageRole");
                });

            // connection name
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>,
                    UserClaimsPrincipalFactory<AppUser, AppRole>>();


            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
             .AddDefaultUI()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();



            services.AddIdentityCore<Employee>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                // causes error "No IUserTwoFactorTokenProvider<TUser> named 'Default' is registered." on .GenerateEmailConfirmationTokenAsync()
                //.AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddTransient<IMakeReservationCategory, MakeReservationCategory>();

            services.Configure<MessageSenderOptions>(Configuration.GetSection("Email"));
            services.AddTransient<IEmailSender, EmailService>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("lt-LT");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("lt-LT") };
                options.RequestCultureProviders.Clear();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePagesWithRedirects("/Error");
            app.UseRequestLocalization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
            });

        }
    }
}
