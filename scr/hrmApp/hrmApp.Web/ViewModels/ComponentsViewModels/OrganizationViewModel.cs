using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hrmApp.Web.ViewModels.ComponentsViewModels
{
    public class OrganizationViewModel
    {
        public int EmployeeId { get; set; }
        public string Message { get; set; }
        public IEnumerable<SelectListItem> Organizations { get; set; }
        public int SelectedOrganizationId { get; set; }
    }
}