using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class ProjectOrganizationDTO : BaseDTO
    {
        public int ProjectId { get; set; }
        public int OrganizationId { get; set; }

        public ProjectDTO Project { get; set; }
        public OrganizationDTO Organization { get; set; }
    }
}
