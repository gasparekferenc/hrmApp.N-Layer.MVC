using System;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO.Base;

namespace hrmApp.Web.DTO
{
    public class HistoryDTO : BaseDTO
    {
        [Display(Name = "Bejegyzés")]
        public string Entry { get; set; }

        public int EntryType { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Bejegyzés dátuma")]
        public DateTime EntryDate { get; set; }

        // True, ha a bejegyzést az ApplicationtUser készítette
        // False, ha a bejegyzés a munkavállaló adataiban történt változás
        // követése miatt történik (log)
        public bool AppUserEntry { get; set; }

        public int? DocumentId { get; set; }

        public bool IsReminder { get; set; }

        public DateTime? DeadlineDate { get; set; }


        public int EmployeeId { get; set; }
        public EmployeeDTO Employee { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUserDTO ApplicationUser { get; set; }

    }
}