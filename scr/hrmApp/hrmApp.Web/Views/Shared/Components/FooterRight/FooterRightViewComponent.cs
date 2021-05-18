using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.FooterRight
{
    public class FooterRightViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}