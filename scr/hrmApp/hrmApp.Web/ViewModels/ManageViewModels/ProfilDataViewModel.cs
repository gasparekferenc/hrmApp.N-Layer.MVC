using System.ComponentModel.DataAnnotations;

namespace hrmApp.Web.ViewModels.ManageViewModels
{
    public class ProfilDataViewModel
    {
        public string Username { get; set; }

        [Display(Name = "Vezetéknév")]
        public string SurName { get; set; }

        [Display(Name = "Keresztnév")]
        public string ForeName { get; set; }

        [Required(ErrorMessage = "Kötelező!")]
        [EmailAddress(ErrorMessage = "Kötelező!")]
        [Display(Name = "Email cím:")]
        public string Email { get; set; }

        [Display(Name = "Telefonszám")]
        public string PhoneNumber { get; set; }
        public string StatusMessage { get; set; }
    }
}
