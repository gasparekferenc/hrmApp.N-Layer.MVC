using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hrmApp.Web.ViewModels
{
    public class AssignViewModel
    {
        public string ApplicationUserId { get; set; }

        [Display(Name = "Felhasználó neve")]
        public string UserName { get; set; }
        public IEnumerable<SelectListItem> Organizations { set; get; }
        public int[] AssignOrganizationIds { set; get; }
    }
}