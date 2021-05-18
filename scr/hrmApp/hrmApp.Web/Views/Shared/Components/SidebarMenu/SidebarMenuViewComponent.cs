using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.SidebarMenu
{
    public class SidebarMenuViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke() => View();

        // private readonly IAuthorizationService _authorizationService;
        // public SidebarMenuViewComponent(IAuthorizationService authorizationService)
        // {
        //     _authorizationService = authorizationService;
        // }



        // public async Task<IViewComponentResult> InvokeAsync()
        // {

        //     var isAuthorized = await _authorizationService.AuthorizeAsync(UserClaimsPrincipal, "admin");
        //     var a = isAuthorized.Succeeded;

        //     return View();
        // }
    }
}