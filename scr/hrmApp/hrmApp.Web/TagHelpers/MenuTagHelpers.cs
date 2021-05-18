
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

// for AdminLTE SideMenu open/close activate handler

namespace hrmApp.Web.TagHelpers
{
    [HtmlTargetElement("li", Attributes = "[class='nav-item has-treeview']")]
    [HtmlTargetElement("a", Attributes = "[class='nav-link']")]
    public class MenuTagHelper : TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var pageController = (string)ViewContext.RouteData.Values["controller"];
            var pageAction = ViewContext.RouteData.Values["action"];

            var content = context.AllAttributes;
            var childContent = await output.GetChildContentAsync();

            content.TryGetAttribute("class", out TagHelperAttribute menuClass);
            string menuType = menuClass.Value.ToString();

            // Activate menu item
            if (menuType.Equals("nav-link"))
            {
                context.AllAttributes.TryGetAttribute("href", out TagHelperAttribute menuLink);
                string menuRoute = menuLink.Value.ToString();

                if (menuRoute.Equals("/" + pageController))
                {
                    output.Attributes.SetAttribute("class", "nav-link active bg-gray");
                }
            }

            // Activate and open involved menu item of up level
            if (menuType.Equals("nav-item has-treeview"))
            {
                string menuRoute = childContent.GetContent();

                if (menuRoute.Contains("\"/" + pageController + "\""))
                {
                    output.Attributes.SetAttribute("class", "nav-item has-treeview menu-is-opening menu-open");
                }
                else
                {
                    output.Attributes.SetAttribute("class", "nav-item has-treeview");
                }
            }
        }
    }
}
