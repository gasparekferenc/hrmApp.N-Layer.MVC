using System.Collections.Generic;
using hrmApp.Web.DTO;

namespace hrmApp.Web.ViewModels.ComponentsViewModels
{
    public class ProjectViewModel
    {
        public int CurrentProjectId { get; set; }
        public string CurrentProjectName { get; set; }

        public List<ProjectDTO> ProjectsOfUser { get; set; }
    }
}