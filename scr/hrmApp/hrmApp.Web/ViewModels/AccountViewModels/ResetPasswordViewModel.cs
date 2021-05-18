using System.ComponentModel.DataAnnotations;
using hrmApp.Web.Constants;

namespace hrmApp.Web.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email cím kötelező!")]
        [EmailAddress(ErrorMessage = "Nem megfelelő formátum!!")]
        [Display(Name = "Email cím:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Jelszó kötelező!")]
        [StringLength(100, ErrorMessage = "A jelszó {0} legalább {2} és legfeljebb {1} karakter lehet.", MinimumLength = BaseValues.PaswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítése")]
        [Compare("Password", ErrorMessage = "A 'Jelszó' és a 'Jelszó megerősítése' nem egyezik.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
