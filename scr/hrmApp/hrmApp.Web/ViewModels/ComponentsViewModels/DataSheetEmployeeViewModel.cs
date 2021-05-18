using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hrmApp.Web.ViewModels.ComponentsViewModels
{
    public class DataSheetEmployeeViewModel
    {
        public int CurrentProjectId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }

        public int EmployeeId { get; set; }

        public EmployeeViewModel Employee { get; set; }

        public IEnumerable<SelectListItem> Jobs { get; set; }
        public int SelectedJobId { get; set; }
    }
}