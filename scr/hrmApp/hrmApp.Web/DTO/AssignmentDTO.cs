using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class AssignmentDTO : BaseDTO
    {
        public string ApplicationUserId { get; set; }
        public int OrganizationId { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }
        public OrganizationDTO Organization { get; set; }
    }
}