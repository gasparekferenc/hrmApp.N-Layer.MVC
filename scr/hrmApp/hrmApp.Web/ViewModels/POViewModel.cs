using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hrmApp.Web.ViewModels
{
    public class POViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<SelectListItem> Organizations { set; get; }
        public int[] AssignOrganizationIds { set; get; }
        public int[] RelatedOrganizationIds { set; get; }
        public string[] RelatedOrganizationNames { set; get; }
    }
}