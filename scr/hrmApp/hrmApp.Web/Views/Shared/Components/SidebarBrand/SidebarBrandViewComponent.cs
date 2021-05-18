using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.SidebarBrand
{
    public class SidebarBrandViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}