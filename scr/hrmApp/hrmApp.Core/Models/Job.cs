/*
Munkavállaló munkaköre
*/

using System.Collections.Generic;
using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class Job : BaseEntityConcurrency
    {
        public string JobName { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; } = true;
        // Egyéni rendezési sorrend lehetősége
        public int PreferOrder { get; set; } = 100;

        public ICollection<Employee> Employees { get; set; }
    }
}