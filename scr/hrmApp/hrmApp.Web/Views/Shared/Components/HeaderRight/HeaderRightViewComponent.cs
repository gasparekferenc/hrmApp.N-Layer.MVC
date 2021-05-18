using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.HeaderRight
{
    public class HeaderRightViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}