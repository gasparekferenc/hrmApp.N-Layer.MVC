using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.Constants;
using hrmApp.Web.DTO;
using hrmApp.Web.Extensions;
using hrmApp.Web.ViewModels;

namespace hrmApp.Web.Controllers
{
    [Authorize(Policy = PolicyNames.ProjectOrganizationHendler)]
    public class ProjectOrganizationController : Controller
    {
        private readonly IProjectOrganizationService _projectOrganizationService;
        private readonly IProjectService _projectService;
        private readonly IOrganizationService _organizationService;
        private readonly IPOEmployeeService _pOEmployeeService;
        private readonly IMapper _mapper;
        public ProjectOrganizationController(
                                    IProjectOrganizationService projectOrganizationService,
                                    IProjectService projectService,
                                    IOrganizationService organizationService,
                                    IPOEmployeeService pOEmployeeService,
                                    IMapper mapper)
        {
            _projectOrganizationService = projectOrganizationService;
            _projectService = projectService;
            _organizationService = organizationService;
            _pOEmployeeService = pOEmployeeService;
            _mapper = mapper;
        }

        // GET: Assignment --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllWithIncludesAsync();
            var projectsDTO = _mapper.Map<IEnumerable<ProjectDTO>>(projects);

            return View(projectsDTO);
        }

        // GET: Assignment/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            // TODO hibakezelés...
            if (id == null) { return BadRequest(); }

            var project = await _projectService.GetWithIncludesByIdAsync((int)id);

            if (project == null) { return NotFound(); }

            POViewModel viewModel = await AssemblePOViewModel(project);

            TempData.Put<int[]>("key", viewModel.RelatedOrganizationIds.ToArray());

            return View(viewModel);
        }

        // POST: Project/Edit/5 ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, int[] assignOrganizationIds, [Bind("RelatedOrganizationIds")] POViewModel pOViewModel)
        public async Task<IActionResult> Edit(int id, int[] assignOrganizationIds)
        {
            // TODO hibakezelés...
            // if (id == null) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                var relatedOrganizationIds = TempData.Get<int[]>("key");
                // Ha lejárt a Session vissza a kályhához...
                if (relatedOrganizationIds == null)
                {
                    // üzenet a felhasználónak, hogy mi is történt
                    return View(nameof(Index));
                }
                await UpdateProjectOrganizations(id, assignOrganizationIds, relatedOrganizationIds);

                HttpContext.Session.SetInt32(SessionKeys.ProjectIdSessionKey, (int)id);

                return RedirectToAction(nameof(Index));
            }

            // Model.State invalid
            return View();
        }


        // -------------------------------- Services -----------------------------------------------

        private async Task UpdateProjectOrganizations(int projectId,
                                                      int[] assignOrganizationIds,
                                                      int[] relatedOrganizationIds)
        {
            // A törlendők listája        
            List<ProjectOrganizationDTO> toBeDeletedPoDTO = new List<ProjectOrganizationDTO>();
            List<int> toBeAddedPoIds = assignOrganizationIds.ToList();

            // Nem vizsgálom, hogy változozz-e az Adatbázis tartalom => Last/Client Win
            var originPOs = await _projectOrganizationService.GetAllByProjectIdAsync(projectId);
            var originPOsDTO = _mapper.Map<IEnumerable<ProjectOrganizationDTO>>(originPOs);

            foreach (var originPO in originPOsDTO)
            {
                var id = originPO.OrganizationId;
                if (!relatedOrganizationIds.Contains(id))
                {
                    if (toBeAddedPoIds.Contains(id))
                    {
                        toBeAddedPoIds.Remove(id);
                    }
                    else
                    {
                        toBeDeletedPoDTO.Add(originPO);
                    }
                }
                // else ág - Ha a Szervezet már kapcsolódik nem módosítjuk
            }
            // Ha van, vagy marad hozzáadandó, akkor hozzáadjuk
            if (toBeAddedPoIds.Any())
            {
                var toBeAddedPoDTO = toBeAddedPoIds
                                            .Select(id => new ProjectOrganizationDTO
                                            {
                                                ProjectId = projectId,
                                                OrganizationId = id
                                            });
                Log.Information("_projectOrganizationService.AddRangeAsync(newProjectOrganizations)...");
                var toBeAddedPO = _mapper.Map<IEnumerable<ProjectOrganization>>(toBeAddedPoDTO);
                await _projectOrganizationService.AddRangeAsync(toBeAddedPO);
            }
            if (toBeDeletedPoDTO.Any())
            {
                Log.Information("_projectOrganizationService.RemoveRange(assignments)...");
                var toBeDeletedPO = _mapper.Map<IEnumerable<ProjectOrganization>>(toBeDeletedPoDTO);
                _projectOrganizationService.RemoveRange(toBeDeletedPO);
            }
        }

        // --------------------------------------------------------------------------------------
        private async Task<POViewModel> AssemblePOViewModel(Project project)
        {
            var projectDTO = _mapper.Map<ProjectDTO>(project);
            var projectId = projectDTO.Id;
            // Get all Organization Id-s assigned to current Project
            var assignedOrganizationIds = projectDTO.ProjectOrganizations
                                        .Select(o => o.OrganizationId)
                                        .ToList();
            // Get all Organizations 
            var organizations = (await _organizationService.GetAllAsync())
                                        .OrderBy(x => x.OrganizationName);
            var organizationsDTO = _mapper.Map<IEnumerable<OrganizationDTO>>(organizations).ToList();

            // Get all Organizations already is relationed 
            var relatedOrganizations = await _organizationService.GetRelatedIdsByProjectIdAsync(projectId);

            var relatedOrganizationIds = relatedOrganizations.Select(o => o.Id);
            var relatedOrganizationNames = relatedOrganizations.Select(o => o.OrganizationName);

            if (relatedOrganizations.Any())
            {
                foreach (var relatedOrganizationId in relatedOrganizationIds)
                {
                    assignedOrganizationIds.Remove(relatedOrganizationId);
                    organizationsDTO.Remove(organizationsDTO.
                                            SingleOrDefault(o => o.Id == relatedOrganizationId));
                }
            }

            var viewModel = new POViewModel
            {
                ProjectId = projectDTO.Id,
                ProjectName = projectDTO.ProjectName,
                Organizations = new SelectList(
                                organizationsDTO,
                                nameof(OrganizationDTO.Id),
                                nameof(OrganizationDTO.OrganizationName)),
                AssignOrganizationIds = assignedOrganizationIds.ToArray(),
                RelatedOrganizationIds = relatedOrganizationIds.ToArray(),
                RelatedOrganizationNames = relatedOrganizationNames.ToArray()
            };

            return viewModel;
        }
    }
}