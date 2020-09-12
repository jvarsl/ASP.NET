using System;
using System.Collections.Generic;
using System.Text;
using SalonWithRazor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SalonWithRazor.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
    AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppRole> AppRole { get; set; }
        public virtual DbSet<AppUserRole> AppUserRole { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeePicture> EmployeePictures { get; set; }
        public virtual DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public virtual DbSet<JobTitle> JobTitles { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationComment> ReservationComments { get; set; }
        public virtual DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public virtual DbSet<Salon> Salons { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceJobTitle> ServiceJobTitles { get; set; }
        public virtual DbSet<ServiceReservation> ServiceReservations { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategory { get; set; }
        //for sp_:
        //[NotMapped] not querying DB. In future EF Core versions check [Keyless] as well
        public virtual DbSet<sp_Hours24Table> sp_Hours24Tables { get; set; }
        public virtual DbSet<sp_LastTimeCheck> sp_LastTimeChecks { get; set; }
        public virtual DbSet<EmployeeAppealSalon> EmployeeAppealSalons { get; set; }
        public virtual DbSet<StaffSalon> StaffSalons { get; set; }
        public virtual DbSet<SalonSchedule> SalonSchedules { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.AppUserRole)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

    }
}
