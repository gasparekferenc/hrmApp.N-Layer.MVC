using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class POEmployeeDTO : BaseDTO
    {
        public int ProjectOrganizationId { get; set; }
        public int EmployeeId { get; set; }

        public ProjectOrganizationDTO ProjectOrganization { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
