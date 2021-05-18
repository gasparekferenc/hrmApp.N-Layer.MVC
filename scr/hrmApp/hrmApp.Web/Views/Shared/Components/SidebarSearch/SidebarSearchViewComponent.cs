using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.SidebarSearch
{
    public class SidebarSearchViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}