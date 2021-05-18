using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.Header
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}