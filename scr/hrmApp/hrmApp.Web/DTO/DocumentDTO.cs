using System;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class DocumentDTO : BaseDTO
    {
        [Display(Name = "Dokumentum megjelenő neve")]
        public string DocDisplayName { get; set; }

        [Display(Name = "Path")]
        public string DocPath { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Dokumentum feltöltésének időpopntja")]
        public DateTime UploadedTimeStamp { get; set; }

        [Display(Name = "Megjegyzés")]
        public string Description { get; set; }

        public int EmployeeId { get; set; }
        public int DocTypeId { get; set; }

        public EmployeeDTO Employee { get; set; }
        public DocTypeDTO DocType { get; set; }
    }
}