using System.ComponentModel.DataAnnotations;

namespace hrmApp.Web.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email kötelező!")]
        [EmailAddress(ErrorMessage = "Nem megfelelő formátum!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
