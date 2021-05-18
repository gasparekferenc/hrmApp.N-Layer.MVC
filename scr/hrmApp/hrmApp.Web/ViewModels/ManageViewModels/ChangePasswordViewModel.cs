using System.ComponentModel.DataAnnotations;
using hrmApp.Web.Constants;

namespace hrmApp.Web.ViewModels.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Kötelező!")]
        [DataType(DataType.Password)]
        [Display(Name = "Jelenlegi jelszó")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Kötelező!")]
        [StringLength(100, ErrorMessage = "A jelszó {0} legalább {2} és legfeljebb {1} karakter lehet.", MinimumLength = BaseValues.PaswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó megerősítése")]
        [Compare("NewPassword", ErrorMessage = "Az 'Új jelszó' és az 'Új jelszó megerősítése' nem egyezik!")]
        public string ConfirmPassword { get; set; }
        public string StatusMessage { get; set; }
    }
}
