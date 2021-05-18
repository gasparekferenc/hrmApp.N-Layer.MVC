using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Serilog;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.DTO;
using hrmApp.Web.ViewModels;


namespace hrmApp.Web.Controllers
{
    [Authorize(Policy = PolicyNames.AssignmentHandler)]
    public class AssignmentController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IOrganizationService _organizationService;
        private readonly IAssignmentService _assignmentService;
        private readonly IMapper _mapper;
        public AssignmentController(
                                    IApplicationUserService applicationUserService,
                                    IOrganizationService organizationService,
                                    IAssignmentService assignmentService,
                                    IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _organizationService = organizationService;
            _assignmentService = assignmentService;
            _mapper = mapper;
        }

        // GET: Assignment --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var usersOfOrganizations = await _applicationUserService.GetAllWithIncludesAsync();
            var usersOfOrganizationsDTO = _mapper.Map<IEnumerable<ApplicationUserDTO>>(usersOfOrganizations);

            return View(usersOfOrganizationsDTO);
        }

        // GET: Assignment/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) { return BadRequest(); }

            var applicationUser = await _applicationUserService.GetWithIncludesByIdAsync(id);

            if (applicationUser == null) { return NotFound(); }

            var viewModel = await AssembleAssignViewModel(applicationUser);

            return View(viewModel);
        }

        // POST: Project/Edit/5 ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, int[] assignOrganizationIds)
        {
            if (id == null) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                await UpdateAssignments(id, assignOrganizationIds);
                return RedirectToAction(nameof(Index));
            }

            // Model.State invalid
            return View();
        }


        // -------------------------------- Services -----------------------------------------------

        private async Task UpdateAssignments(string applicationUserId, int[] assignOrganizationIds)
        {
            // Minden bejegyzést törlhetünk, mert ez csak hozzáférést
            // biztosít a User számára a Szervezet kezeléséhez, ez egy jogosultság.
            var assignments = (await _assignmentService.GetAllAsync())
                 .Where(x => x.ApplicationUserId == applicationUserId);

            _assignmentService.RemoveRange(assignments);

            Log.Information("_assignmentService.RemoveRange(assignments)...");

            var newAssignemts = new List<Assignment>();
            foreach (var organizationId in assignOrganizationIds)
            {
                newAssignemts.Add(new Assignment
                {
                    ApplicationUserId = applicationUserId,
                    OrganizationId = organizationId
                }
                );
            }

            await _assignmentService.AddRangeAsync(newAssignemts);

            Log.Information("_assignmentService.AddRangeAsync(newAssignemts)...");

        }

        // --------------------------------------------------------------------------------------
        private async Task<AssignViewModel> AssembleAssignViewModel(ApplicationUser applicationUser)
        {
            var applicationUserDTO = _mapper.Map<ApplicationUserDTO>(applicationUser);

            var organizations = (await _organizationService.GetAllAsync())
                                        .OrderBy(x => x.OrganizationName);
            var organizationsDTO = _mapper.Map<IEnumerable<OrganizationDTO>>(organizations);

            var viewModel = new AssignViewModel
            {
                ApplicationUserId = applicationUserDTO.Id,
                UserName = applicationUserDTO.UserName,
                Organizations = new SelectList(
                                organizationsDTO,
                                nameof(OrganizationDTO.Id),
                                nameof(OrganizationDTO.OrganizationName)),
                AssignOrganizationIds = applicationUserDTO.Assignments
                                        .Select(o => o.OrganizationId)
                                        .ToArray()
            };

            return viewModel;
        }
    }
}