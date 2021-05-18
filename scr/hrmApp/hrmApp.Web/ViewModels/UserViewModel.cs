using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hrmApp.Web.ViewModels
{
    public class UserViewModel
    {

        public string Id { get; set; }

        [Required]
        [Display(Name = "Felhasználó neve")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Vezetéknév")]
        public string SurName { get; set; }

        [Required]
        [Display(Name = "Keresztnév")]
        public string ForeName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Jelszó megerősítése")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Megjegyzés")]
        public string Description { get; set; }

        [Display(Name = "Hozzáférés felfüggesztés")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Email megerősítés")]
        public bool EmailConfirmed { get; set; }

        public byte[] RowVersion { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }

        public List<UserRoleViewModel> UserRoles { get; set; }
    }
}
