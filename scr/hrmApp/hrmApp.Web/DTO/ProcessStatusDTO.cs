using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class ProcessStatusDTO : BaseConcurrencyDTO
    {
        [Display(Name = "Státusznév")]
        public string StatusName { get; set; }

        [Display(Name = "Leírás")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Egyéni rendezési sorrend")]
        public int PreferOrder { get; set; } = 100;

        public ICollection<EmployeeDTO> Employees { get; set; }
    }
}