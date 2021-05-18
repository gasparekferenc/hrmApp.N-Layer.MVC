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
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace hrmApp.Web.Views.Shared.Components.DataSheetOrganization
{
    public class DataSheetOrganizationViewComponent : ViewComponent
    {
        private readonly IOrganizationService _organizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public DataSheetOrganizationViewComponent(
            IOrganizationService organizationService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _organizationService = organizationService;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(int employeeId)
        {

            int currentProjectId = HttpContext.Session.GetInt32(SessionKeys.ProjectIdSessionKey) ?? 0;
            if (currentProjectId == 0)
            {
                throw new ArgumentException();
            }

            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            var userId = user.Id;

            var message = (string)TempData["ChangeOrganizationMessage"] ?? "";

            var organizations = (await _organizationService.GetByUserIdandProjectIdAsync(userId, currentProjectId))
                                    .OrderBy(o => o.OrganizationName)
                                    .ToList();
            var organizationsDTO = _mapper.Map<IEnumerable<OrganizationDTO>>(organizations);

            var organization = await _organizationService.GetByEmployeeIdAsync(employeeId);

            var viewModel = new OrganizationViewModel
            {
                EmployeeId = employeeId,
                Message = message,
                Organizations = new SelectList(
                                        organizationsDTO,
                                        nameof(OrganizationDTO.Id),
                                        nameof(OrganizationDTO.OrganizationName)
                ),
                SelectedOrganizationId = organization.Id
            };

            return View(viewModel);
        }


    }
}