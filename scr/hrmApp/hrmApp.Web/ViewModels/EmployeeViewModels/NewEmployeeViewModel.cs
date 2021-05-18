using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hrmApp.Web.ViewModels.EmployeeViewModels
{
    public class NewEmployeeViewModel
    {
        public int CurrentProjectId { get; set; }
        public string UserId { get; set; }

        [Display(Name = "TAJ szám")]
        public string SSNumber { get; set; }

        [Display(Name = "Vezetéknév")]
        public string SurName { get; set; }

        [Display(Name = "Keresztnév")]
        public string ForeName { get; set; }

        [Display(Name = "Születési hely")]
        public string Birthplace { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Születési dátum")]
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<SelectListItem> Organizations { get; set; }
        public int SelectedOrganizationId { get; set; }

        public IEnumerable<SelectListItem> Jobs { get; set; }
        public int SelectedJobId { get; set; }
        public IEnumerable<SelectListItem> ProcessStatuses { get; set; }
        public int SelectedProcessStatusId { get; set; }

    }
}