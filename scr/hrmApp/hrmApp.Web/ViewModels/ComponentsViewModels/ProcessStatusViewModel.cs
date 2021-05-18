using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hrmApp.Web.ViewModels.ComponentsViewModels
{
    public class ProcessStatusViewModel
    {
        public int EmployeeId { get; set; }
        public string Message { get; set; }
        public IEnumerable<SelectListItem> ProcessStatuses { get; set; }
        public int SelectedProcessStatusId { get; set; }
    }
}