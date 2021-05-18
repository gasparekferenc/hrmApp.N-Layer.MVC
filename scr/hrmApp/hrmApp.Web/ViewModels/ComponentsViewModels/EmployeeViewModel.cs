using System;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO;

// Employee's properties, that the App currently implement

namespace hrmApp.Web.ViewModels.ComponentsViewModels
{
    public class EmployeeViewModel
    {

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

        [Display(Name = "Anyja neve")]
        public string MothersName { get; set; }

        [Display(Name = "TAJ száma")]
        public string SSNumber { get; set; }


        [Display(Name = "Telefonszáma(i)")]
        public string PhoneNumber { get; set; }

        [Display(Name = "E-mail cím")]
        public string Email { get; set; }

        [Display(Name = "Utazási támogatásra jogosult?")]
        public bool TravelAllowance { get; set; }

        [Display(Name = "Megjegyzés")]
        public string Description { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Belépés dátuma")]
        public DateTime? StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Kilépés dátuma")]
        public DateTime? EndDate { get; set; }

        public int JobId { get; set; }
        public JobDTO Job { get; set; }
    }
}