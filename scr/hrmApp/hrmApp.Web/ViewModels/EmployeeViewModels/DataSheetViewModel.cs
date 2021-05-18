using hrmApp.Web.DTO;

namespace hrmApp.Web.ViewModels.EmployeeViewModels
{
    public class DataSheetViewModel
    {
        public int CurrentProjectId { get; set; }
        public string UserId { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}