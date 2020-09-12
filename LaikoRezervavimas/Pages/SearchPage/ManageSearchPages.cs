using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalonWithRazor.Pages.SearchPage
{
    public static class ManageSearchPages
    {
        public static string Index => "Index";

        public static string SearchSalons => "SearchSalons";

        public static string SearchServices => "SearchServices";

        public static string SearchEmployeesList => "SearchEmployeesList";
        public static string SearchServicesList => "SearchServicesList";


        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string SearchSalonsNavClass(ViewContext viewContext) => PageNavClass(viewContext, SearchSalons);

        public static string SearchServicesNavClass(ViewContext viewContext) => PageNavClass(viewContext, SearchServices);

        public static string SearchEmployeesListNavClass(ViewContext viewContext) => PageNavClass(viewContext, SearchEmployeesList);

        public static string SearchServicesListNavClass(ViewContext viewContext) => PageNavClass(viewContext, SearchServicesList);


        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
