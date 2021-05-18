using System;
using System.ComponentModel.DataAnnotations;

namespace hrmApp.Web.ViewModels
{
    public class ClaimsViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Követelmény név")]
        public string ClaimName { get; set; }
        public Boolean HasClaim { get; set; }
    }
}
