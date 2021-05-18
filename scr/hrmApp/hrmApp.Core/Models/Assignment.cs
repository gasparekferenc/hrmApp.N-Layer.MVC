/*
Az egyes szervezeteket (Organization) és felhasználók (ApplicationsUser)
összerendelése.
kapcsolat típus: many-to-many
 */

using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class Assignment : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public int OrganizationId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Organization Organization { get; set; }
    }
}