using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class ProjectDTO : BaseConcurrencyDTO
    {

        [Display(Name = "Projekt neve")]
        public string ProjectName { get; set; }

        [Display(Name = "Létszámkeret")]
        public int NumberOfEmployee { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Kezdés dátuma")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Befejezés dátuma")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leírás")]
        public string Description { get; set; }

        [Display(Name = "Aktív")]
        public bool IsActive { get; set; } = true;

        public ICollection<ProjectOrganizationDTO> ProjectOrganizations { get; set; }
    }
}
