using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Serilog;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.Constants;
using hrmApp.Web.DTO;
using hrmApp.Web.ViewModels.EmployeeViewModels;

namespace hrmApp.Web.Controllers
{
    [Authorize(Policy = PolicyNames.ReportHandler)]
    #region ReportController : Controller
    public class ReportController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IOrganizationService _organizationService;
        private readonly IProjectOrganizationService _projectOrganizationService;
        private readonly IPOEmployeeService _pOEmployeeService;
        private readonly IJobService _jobService;
        private readonly IProcessStatusService _processStatusService;
        private readonly IDocumentService _documentService;
        private readonly IDocTypeService _docTypeService;
        private readonly IHistoryService _historyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ReportController(
            IEmployeeService employeeService,
            IProjectService projectService,
            IOrganizationService organizationService,
            IProjectOrganizationService projectOrganizationService,
            IPOEmployeeService pOEmployeeService,
            IJobService jobService,
            IProcessStatusService processStatusService,
            IDocumentService documentService,
            IDocTypeService docTypeService,
            IHistoryService historyService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IFileService fileService
            )
        {
            _employeeService = employeeService;
            _projectService = projectService;
            _organizationService = organizationService;
            _projectOrganizationService = projectOrganizationService;
            _pOEmployeeService = pOEmployeeService;
            _jobService = jobService;
            _processStatusService = processStatusService;
            _documentService = documentService;
            _docTypeService = docTypeService;
            _historyService = historyService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _fileService = fileService;
        }
        #endregion

        #region GET:  Employee/Index

        // GET: Employee/Index -----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                // ha User nincs bejelentkezve... (DE ide normál esetben nem kerül a vezérlés)
                return RedirectToAction(nameof(AccountController.Login), "Account", "/Employee/Index");
            }

            int currentProjectId = HttpContext.Session.GetInt32(SessionKeys.ProjectIdSessionKey) ?? 0;
            if (currentProjectId == 0)
            {
                // Ha a User-hez még nincs projekt rendelve
                return View(new EmployeeIndexViewModel { CurrentProjectId = 0 });
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            Log.Information($"");

            var employees = (await _employeeService.GetAllEmployeesAysnc());
            //.Distinct();

            EmployeeIndexViewModel viewModel = new EmployeeIndexViewModel
            {
                CurrentProjectId = currentProjectId,
                Employees = _mapper.Map<List<EmployeeDTO>>(employees)
            };

            return View(viewModel);
        }

        #endregion



    }
}