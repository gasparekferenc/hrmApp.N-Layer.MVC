using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Core.PagedList;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Controllers
{
    [Authorize(Policy = PolicyNames.BaseDataHandler)]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        #region GET: Project
        // GET: Project --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllAsync();
            var projectDTOs = _mapper.Map<IEnumerable<ProjectDTO>>(projects);

            return View(projectDTOs);
        }
        #endregion

        #region GET: Project/PagedList?term=2019&pageIndex=1&pageSize=3
        // This is ignored, cause DataTables will better(, but work ok)
        // Server side pagination... -------------------------------------------
        // GET: Project/PagedList?pageIndex=1&pageSize=3
        // GET: Project/PagedList?term=2014&pageIndex=1&pageSize=3
        public async Task<IActionResult> PagedList(int? pageIndex,
                                                   int? pageSize,
                                                   string term = "")
        {
            var pagedProjects = await _projectService.GetPagedListAsync(
                predicate: p => p.ProjectName.Contains(term),
                orderBy: source => source.OrderByDescending(p => p.ProjectName),
                pageIndex: pageIndex ?? 1,
                pageSize: pageSize ?? Paging.PageSize
                );

            var pagedProjectDTOs = _mapper.Map<IPagedList<Project>,
                                               IPagedListDTO<ProjectDTO>>
                                               (pagedProjects);

            return View(pagedProjectDTOs);
        }
        #endregion

        #region GET: Project/Details/5
        // GET: Project/Details/5 ----------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var project = await _projectService.GetByIdAysnc((int)id);
            if (project == null)
            {
                return NotFound();
            }
            var projectDTO = _mapper.Map<ProjectDTO>(project);
            return View(projectDTO);
        }
        #endregion

        #region GET: Project/Create

        // GET: Project/Create -----------------------------------------------
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region POST: Project/Create

        // POST: Project/Create ------------------------------------------------
        // To protect from overposting attacks, enable the specific properties
        // you want to bind to, for more details,
        // see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectDTO projectDTO)
        {
            if (ModelState.IsValid)
            {
                var project = _mapper.Map<Project>(projectDTO);
                await _projectService.AddAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(projectDTO);
        }
        #endregion

        #region GET: Project/Edit/5
        // GET: Project/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var project = await _projectService.GetByIdAysnc((int)id);
            if (project == null)
            {
                return NotFound();
            }
            var projectDTO = _mapper.Map<ProjectDTO>(project);
            return View(projectDTO);
        }
        #endregion

        #region POST: Project/Edit/5            
        // POST: Project/Edit/5 ------------------------------------------------
        // To protect from overposting attacks, enable the specific properties
        // you want to bind to, for more details,
        // see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectDTO projectDTO)
        {
            if (id != projectDTO.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var project = _mapper.Map<Project>(projectDTO);
                try
                {
                    await _projectService.UpdateAsync(project);
                }
                // Database concurrency
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Information("Concurrency catching.", ex);

                    ConcurrencyHandler(ex, ref projectDTO);

                    // Database Conflict or Deleted record
                    // Back to Edit page with error message
                    return View(projectDTO);
                }
                // update success
                return RedirectToAction(nameof(Index));
            }
            // Model.State invalid
            return View(projectDTO);
        }
        #endregion

        #region GET: Project/Delete/5            
        // GET: Project/Delete/5 -----------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var project = await _projectService.GetWithPOByIdAsync((int)id);
            if (project == null)
            {
                return NotFound();
            }

            var projectDTO = _mapper.Map<ProjectDTO>(project);
            return View(projectDTO);
        }
        #endregion

        #region POST: Project/Delete/5        
        // POST: Project/Delete/5 ----------------------------------------------
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, ProjectDTO projectDTO)
        {
            if (id != projectDTO.Id)
            {
                return BadRequest();
            }

            var project = await _projectService.GetWithPOByIdAsync(id);

            if (project == null)
            {
                //return NotFound();
                ModelState.AddModelError(string.Empty,
                                "A Projektet egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
                return View(projectDTO);
            }

            if (project.ProjectOrganizations.Any())
            {
                Log.Information("There is related entity.");
                ModelState.AddModelError(string.Empty,
                                "A Projekt nem törölhető! " +
                                "A Projektre hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");
                // Back to Delete page with error message
                return View(_mapper.Map<ProjectDTO>(project));
            }

            try
            {
                await _projectService.RemoveAsync(project);
            }
            // Database constraint voliation
            catch (DbUpdateException ex)
            {
                Log.Information("Database constraint voliation.", ex);

                ModelState.Clear();
                ModelState.AddModelError(string.Empty,
                                "A Projekt nem törölhető! " +
                                "A Projektre hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");

                // Back to Delete page with error message
                return View(projectDTO);
            }
            // update success
            return RedirectToAction(nameof(Index));

        }
        #endregion


        #region helpermethods        
        // ---------------------------- helper methods ----------------------------

        private void ConcurrencyHandler(DbUpdateConcurrencyException ex,
                                       ref ProjectDTO projectDTO)
        {
            var exceptionEntry = ex.Entries.Single();
            var clientValues = (Project)exceptionEntry.Entity;
            var databaseEntry = exceptionEntry.GetDatabaseValues();

            ModelState.Clear();

            if (databaseEntry == null)
            {
                ModelState.AddModelError(string.Empty,
                                "A módosításokat nem lehet menteni." +
                                "A Projektet egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
            }
            else
            {
                var databaseValues = (Project)databaseEntry.ToObject();

                projectDTO.RowVersion = (byte[])databaseValues.RowVersion;
                ModelState.Remove("RowVersion");

                ModelState.AddModelError(string.Empty,
                                "A szerkesztés alatt a Projekt adatait " +
                                "egy másik felhasználó megváltoztatta." +
                                "\nPirossal az adatbázisban jelenleg tárolt," +
                                " eltérő adatok láthatóak." +
                                "\nA [Mentés] gombra kattintva az adatbázis" +
                                " adatai felülíródnak." +
                                "\nA [Vissza] gombra kattintva módosításaid" +
                                " elvesznek.");

                if (databaseValues.ProjectName != clientValues.ProjectName)
                {
                    ModelState.AddModelError(nameof(databaseValues.ProjectName),
                        $"Adatbázisban: {databaseValues.ProjectName}");
                }
                if (databaseValues.NumberOfEmployee != clientValues.NumberOfEmployee)
                {
                    ModelState.AddModelError(nameof(databaseValues.NumberOfEmployee),
                        $"Adatbázisban: {databaseValues.NumberOfEmployee}");
                }
                if (databaseValues.StartDate != clientValues.StartDate)
                {
                    ModelState.AddModelError(nameof(databaseValues.StartDate),
                        $"Adatbázisban: " +
                        string.Format("{0:yyyy.MM.dd}", databaseValues.StartDate));
                }
                if (databaseValues.EndDate != clientValues.EndDate)
                {
                    ModelState.AddModelError(nameof(databaseValues.EndDate),
                        $"Adatbázisban: " +
                        string.Format("{0:yyyy.MM.dd}", databaseValues.EndDate));
                }
                if (databaseValues.Description != clientValues.Description)
                {
                    ModelState.AddModelError(nameof(databaseValues.Description),
                        $"Adatbázisban: {databaseValues.Description}");
                }
                if (databaseValues.IsActive != clientValues.IsActive)
                {
                    ModelState.AddModelError(nameof(databaseValues.IsActive),
                        $"Adatbázisban: {databaseValues.IsActive}");
                }
            }
        }
        #endregion
    }
}
