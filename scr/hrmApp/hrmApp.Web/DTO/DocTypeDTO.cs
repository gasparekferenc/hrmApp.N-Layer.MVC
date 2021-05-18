using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class DocTypeDTO : BaseConcurrencyDTO
    {
        [Display(Name = "Dokumentum típus neve")]
        public string TypeName { get; set; }

        [Display(Name = "Leírás")]
        public string Description { get; set; }

        [Display(Name = "Kötelezően feltöltendő dokumentum elem?")]
        public bool MandatoryElement { get; set; }

        [Display(Name = "Aktív")]
        public bool IsActive { get; set; }

        [Display(Name = "Egyéni rendezési sorrend")]
        public int PreferOrder { get; set; }

        public ICollection<DocumentDTO> Documents { get; set; }

    }
}