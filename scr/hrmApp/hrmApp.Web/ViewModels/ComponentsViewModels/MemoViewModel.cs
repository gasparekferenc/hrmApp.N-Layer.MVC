using System;
using System.ComponentModel.DataAnnotations;

namespace hrmApp.Web.ViewModels.ComponentsViewModels
{
    public class MemoViewModel
    {
        public string ApplicationUserId { get; set; }
        public int EmployeeId { get; set; }
        public string Message { get; set; }

        [Display(Name = "Feljegyzés")]
        public string Entry { get; set; }
        public DateTime EntryDate { get; set; }
        public bool AppUserEntry { get; set; }

        [Display(Name = "Emlékeztető")]
        public bool IsReminder { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = false)]

        public DateTime? DeadlineDate { get; set; }

    }
}