using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.HeaderLeft
{
    public class HeaderLeftViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}