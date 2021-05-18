using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class OrganizationDTO : BaseConcurrencyDTO
    {
        [Display(Name = "Intézmény neve")]
        public string OrganizationName { get; set; }

        [Display(Name = "Település")]
        public string City { get; set; }

        [Display(Name = "Cím: utca, házszám")]
        public string Address { get; set; }

        [Display(Name = "Kapcsolattartó neve")]
        public string ContactName { get; set; }

        [EmailAddress(ErrorMessage = "Nem megfelelő az e-mail cím formátuma!")]
        [Display(Name = "Kapcsolattartó e-mail címe")]
        public string ContactEmail { get; set; }

        [Display(Name = "Kapcsolattartó telefonszáma(i)")]
        public string ContactPhone { get; set; }

        [Display(Name = "Megjegyzés")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Display(Name = "Egyéni rendezési sorrend")]
        public int PreferOrder { get; set; } = 100;


        public ICollection<ProjectOrganizationDTO> ProjectOrganizations { get; set; }

        public ICollection<AssignmentDTO> Assignments { get; set; }
    }
}