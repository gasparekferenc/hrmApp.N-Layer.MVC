using System.ComponentModel.DataAnnotations;

namespace hrmApp.Web.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Szerepkör név")]
        public string RoleName { get; set; }
    }
}
