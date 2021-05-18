using System.Collections.Generic;
using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class ProcessStatus : BaseEntityConcurrency
    {
        public string StatusName { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; } = true;
        public int PreferOrder { get; set; } = 100;

        // One-to-Many => Navigation property: Employees
        public ICollection<Employee> Employees { get; set; }
    }
}