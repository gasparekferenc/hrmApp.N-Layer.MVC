using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.FooterLeft
{
    public class FooterLeftViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}