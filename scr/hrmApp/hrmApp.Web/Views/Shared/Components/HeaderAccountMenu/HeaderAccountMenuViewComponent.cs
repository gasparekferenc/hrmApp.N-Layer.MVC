using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.HeaderAccountMenu
{
    public class HeaderAccountMenuViewComponent : ViewComponent
    {

        public HeaderAccountMenuViewComponent()
        {
        }
        public IViewComponentResult Invoke() => View();
    }
}