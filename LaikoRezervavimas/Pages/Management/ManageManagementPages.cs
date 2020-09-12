using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalonWithRazor.Pages.Management
{
    public static class ManageManagementPages
    {
        public static string Index => "Index";

        public static string AssignStaffEmployees => "AssignStaffEmployees";
        public static string Admin_AssignStaffToSalons => "AssignStaffToSalons";

        public static string Admin_AddCompanies => "AddCompanies";

        public static string Admin_AddSalons => "AddSalons";
        public static string Admin_AddJobTitles => "AddJobTitles";
        public static string Staff_ConfirmEmployeeAppeals => "ConfirmEmployeeAppeals";
        public static string Staff_AddServices => "AddServices";
        public static string Staff_ManageSalons => "ManageSalons";
        public static string Staff_ManageEmployees => "ManageEmployees";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string Admin_AssignStaffEmployeesNavClass(ViewContext viewContext) => PageNavClass(viewContext, AssignStaffEmployees);

        public static string Admin_AssignStaffToSalonsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Admin_AssignStaffToSalons);

        public static string Admin_AddCompaniesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Admin_AddCompanies);

        public static string Admin_AddSalonsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Admin_AddSalons);

        public static string Admin_AddJobTitlesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Admin_AddJobTitles);

        public static string Staff_ConfirmEmployeeAppealsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Staff_ConfirmEmployeeAppeals);

        public static string Staff_AddServicesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Staff_AddServices);

        public static string Staff_ManageSalonsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Staff_ManageSalons);

        public static string Staff_ManageEmployeesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Staff_ManageEmployees);



        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
