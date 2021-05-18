using System.ComponentModel.DataAnnotations;
using hrmApp.Web.Constants;

namespace hrmApp.Web.ViewModels.AccountViewModels
{
    public class ConfirmEmailViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Email cím kötelező!")]
        [EmailAddress(ErrorMessage = "Nem megfelelő formátum!")]
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

        [Display(Name = "GDPR irányelvek elfogadása")]
        public bool GDPRConfirmed { get; set; }

        [Display(Name = "Felhasználói irányelvek elfogadása")]
        public bool TermOfUseConfirmed { get; set; }


        public string Code { get; set; }
    }
}