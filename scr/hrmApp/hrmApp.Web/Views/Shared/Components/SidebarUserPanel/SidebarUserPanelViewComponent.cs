using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.SidebarUserPanel
{
    public class SidebarUserPanelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}