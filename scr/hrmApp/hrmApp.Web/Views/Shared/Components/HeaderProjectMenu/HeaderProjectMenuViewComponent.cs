using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using hrmApp.Web.Constants;
using hrmApp.Web.DTO;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.ViewModels.ComponentsViewModels;


namespace hrmApp.Web.Views.Shared.Components.HeaderProjectMenu
{
    public class HeaderProjectMenuViewComponent : ViewComponent
    {
        private readonly IProjectService _projectService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public HeaderProjectMenuViewComponent(
            IProjectService projectService,
            IApplicationUserService applicationUserService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _projectService = projectService;
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_signInManager.IsSignedIn(UserClaimsPrincipal))
            {
                // ha User nincs bejelentkezve, DE ide normál esetben nem kerül a vezérlés
                return View(new ProjectViewModel { CurrentProjectId = 0 });
            }

            var currentProjectId = HttpContext.Session.GetInt32(SessionKeys.ProjectIdSessionKey) ?? 0;
            if (currentProjectId == 0)
            {
                // Ha a User-hez még nincs projekt rendelve
                return View(new ProjectViewModel { CurrentProjectId = 0 });
            }

            var project = await _projectService.GetByIdAysnc((int)currentProjectId);
            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            var projectsOfUser = await _applicationUserService.GetProjectsOfUserAsync(user.Id);
            ProjectViewModel viewModel = new ProjectViewModel
            {
                CurrentProjectId = project.Id,
                CurrentProjectName = project.ProjectName,
                ProjectsOfUser = _mapper.Map<List<ProjectDTO>>(projectsOfUser)
            };
            return View(viewModel);
        }
    }
}