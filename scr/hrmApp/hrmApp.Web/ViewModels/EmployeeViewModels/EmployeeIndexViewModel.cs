using System.Collections.Generic;
using hrmApp.Web.DTO;

namespace hrmApp.Web.ViewModels.EmployeeViewModels
{
    public class EmployeeIndexViewModel
    {
        public int CurrentProjectId { get; set; }
        public string CurrentProjectName { get; set; }
        public List<EmployeeDTO> Employees { get; set; }
    }
}