
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class JobDTO : BaseConcurrencyDTO
    {

        [Display(Name = "Munkakör")]
        public string JobName { get; set; }

        [Display(Name = "Megjegyzés")]
        public string Description { get; set; }

        [Display(Name = "Aktív")]
        public bool IsActive { get; set; }

        [Display(Name = "Egyéni rendezési sorrend")]
        public int PreferOrder { get; set; }


        public ICollection<EmployeeDTO> Employees { get; set; }
    }
}