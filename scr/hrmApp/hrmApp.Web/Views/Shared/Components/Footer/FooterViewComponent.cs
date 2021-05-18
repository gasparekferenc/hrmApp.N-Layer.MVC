using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.Footer
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}