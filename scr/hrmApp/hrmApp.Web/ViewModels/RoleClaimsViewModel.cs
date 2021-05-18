using System.Collections.Generic;

namespace hrmApp.Web.ViewModels
{
    public class RoleClaimsViewModel : RoleViewModel
    {
        public List<ClaimsViewModel> RoleClaims { get; set; }
    }
}
