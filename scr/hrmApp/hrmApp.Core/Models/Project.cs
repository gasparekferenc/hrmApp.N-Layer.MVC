using System;
using System.Collections.Generic;
using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class Project : BaseEntityConcurrency
    {
        public string ProjectName { get; set; }
        public int NumberOfEmployee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<ProjectOrganization> ProjectOrganizations { get; set; }
    }
}