using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO.Base;

// Not all properties are used up in the app

// Validation => https://docs.fluentvalidation.net/en/latest/index.html

namespace hrmApp.Web.DTO
{
    public class EmployeeDTO : BaseConcurrencyDTO
    {
        [Display(Name = "Vezetéknév")]
        public string SurName { get; set; }

        [Display(Name = "Keresztnév")]
        public string ForeName { get; set; }

        [Display(Name = "Név")]
        public string FullName { get { return SurName + " " + ForeName; } }

        [Display(Name = "Születési hely")]
        public string Birthplace { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Születési dátum")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Állandó lakcím - irányítószám")]
        public string PermPostCode { get; set; }

        [Display(Name = "Állandó lakcím - település")]
        public string PermCity { get; set; }

        [Display(Name = "Állandó lakcím - utca, házszám")]
        public string PermAddress { get; set; }


        [Display(Name = "Tartózkodási cím -irányítószám")]
        public string ResPostCode { get; set; }

        [Display(Name = "Tartózkodási cím - település")]
        public string ResCity { get; set; }

        [Display(Name = "Tartózkodási cím - utca, házszám")]
        public string ResAddress { get; set; }


        [Display(Name = "Legmagasabb iskolai végzettség")]
        public string EduLevel { get; set; }

        [Display(Name = "A végzettséget igazoló dokumentum száma")]
        public string EduDocId { get; set; }

        [Display(Name = "A végzettség megszerzésének dátuma")]
        public DateTime? EduDocDate { get; set; }

        [Display(Name = "A dokumentumot kiállító intézmény neve")]
        public string EduInstitute { get; set; }

        [Display(Name = "Anyja neve")]
        public string MothersName { get; set; }

        [Display(Name = "TAJ száma")]
        public string SSNumber { get; set; }

        [Display(Name = "Adóazonosító jele")]
        public string TINumber { get; set; }

        [Display(Name = "Bankszámla száma")]
        public string BANumber { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Foglalkozás egészségügyi alkalmasság érvényessége")]
        public DateTime? OccValidDate { get; set; }

        [Display(Name = "Telefonszáma(i)")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Kapcsolattartó e-mail címe")]
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

        [Display(Name = "Aktív")]
        public bool IsActive { get; set; }


        public ICollection<POEmployeeDTO> POEmployee { get; set; }

        public int ProcessStatusId { get; set; }
        public ProcessStatusDTO ProcessStatus { get; set; }

        public int JobId { get; set; }
        public JobDTO Job { get; set; }

        public ICollection<HistoryDTO> Histories { get; set; }

        public ICollection<DocumentDTO> Documents { get; set; }
    }
}
