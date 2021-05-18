using System.Collections.Generic;
using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class Organization : BaseEntityConcurrency
    {
        public string OrganizationName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int PreferOrder { get; set; } = 100;

        public ICollection<ProjectOrganization> ProjectOrganizations { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
    }
}