using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.Constants;
using hrmApp.Web.DTO;
using hrmApp.Web.ViewModels.ComponentsViewModels;


namespace hrmApp.Web.Views.Shared.Components.DataSheetEmployee
{
    public class DataSheetEmployeeViewComponent : ViewComponent
    {
        private readonly IEmployeeService _employeeService;
        private readonly IJobService _jobService;
        private readonly IHistoryService _historyService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public DataSheetEmployeeViewComponent(
            IEmployeeService employeeService,
            IJobService jobService,
            IHistoryService historyService,
            IApplicationUserService applicationUserService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _jobService = jobService;
            _historyService = historyService;
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _signInManager = signInManager;
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

            var message = (string)TempData["ChangeEmployeeDataMessage"] ?? "";

            var employee = await _employeeService.GetByIdAysnc(employeeId);
            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);

            var jobs = (await _jobService.GetAllAsync()).OrderBy(j => j.PreferOrder);
            var jobDTO = _mapper.Map<List<JobDTO>>(jobs);

            var viewModel = new DataSheetEmployeeViewModel
            {
                CurrentProjectId = currentProjectId,
                UserId = user.Id,
                Message = message,

                EmployeeId = employeeId,
                Employee = employeeVM,
                Jobs = new SelectList(
                                    jobDTO,
                                    nameof(JobDTO.Id),
                                    nameof(JobDTO.JobName)),
                SelectedJobId = employeeVM.JobId
            };

            return View(viewModel);
        }
    }
}