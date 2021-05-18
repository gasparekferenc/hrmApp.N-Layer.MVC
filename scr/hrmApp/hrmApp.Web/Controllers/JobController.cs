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
using hrmApp.Web.DTO;

namespace hrmApp.Web.Controllers
{
    [Authorize(Policy = PolicyNames.BaseDataHandler)]
    public class JobController : Controller
    {

        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public JobController(IJobService jobService, IMapper mapper)
        {
            _jobService = jobService;
            _mapper = mapper;
        }

        // GET: Job --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var jobs = await _jobService.GetAllAsync();
            var jobDTOs = _mapper.Map<IEnumerable<JobDTO>>(jobs);

            return View(jobDTOs);
        }

        // GET: Job/Details/5 ----------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var job = await _jobService.GetByIdAysnc((int)id);
            if (job == null)
            {
                return NotFound();
            }
            var jobDTO = _mapper.Map<JobDTO>(job);
            return View(jobDTO);
        }

        // GET: Job/Create -----------------------------------------------
        public IActionResult Create()
        {
            return View();
        }

        // POST: Job/Create ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobDTO jobDTO)
        {
            if (ModelState.IsValid)
            {
                var job = _mapper.Map<Job>(jobDTO);
                await _jobService.AddAsync(job);
                return RedirectToAction(nameof(Index));
            }
            return View(jobDTO);
        }

        // GET: Job/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var job = await _jobService.GetByIdAysnc((int)id);
            if (job == null)
            {
                return NotFound();
            }
            var jobDTO = _mapper.Map<JobDTO>(job);
            return View(jobDTO);
        }

        // POST: Job/Edit/5 ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobDTO jobDTO)
        {
            if (id != jobDTO.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var job = _mapper.Map<Job>(jobDTO);
                try
                {
                    await _jobService.UpdateAsync(job);
                }
                // Database concurrency
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Information("Concurrency catching.", ex);

                    ConcurrencyHandler(ex, ref jobDTO);

                    // Database Conflict or Deleted record
                    // Back to Edit page with error message
                    return View(jobDTO);
                }
                // update success
                return RedirectToAction(nameof(Index));
            }
            // Model.State invalid
            return View(jobDTO);
        }


        // GET: Job/Delete/5 -----------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var job = await _jobService.GetWithEployeesByIdAsync((int)id);
            if (job == null)
            {
                return NotFound();
            }

            var jobDTO = _mapper.Map<JobDTO>(job);
            return View(jobDTO);
        }

        // POST: Job/Delete/5 ----------------------------------------------
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, JobDTO jobDTO)
        {
            if (id != jobDTO.Id)
            {
                return BadRequest();
            }

            var job = await _jobService.GetWithEployeesByIdAsync(id);

            if (job == null)
            {
                //return NotFound();
                ModelState.AddModelError(string.Empty,
                                "A Munkakört egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
                return View(jobDTO);
            }

            if (job.Employees.Any())
            {
                Log.Information("There is related entity for this Job.");
                ModelState.AddModelError(string.Empty,
                                "A Munkakör nem törölhető! " +
                                "A Munkakörre hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");
                // Back to Delete page with error message
                return View(_mapper.Map<JobDTO>(job));
            }

            try
            {
                await _jobService.RemoveAsync(job);
            }
            // Database constraint voliation
            catch (DbUpdateException ex)
            {
                Log.Information("Database constraint voliation.", ex);

                ModelState.Clear();
                ModelState.AddModelError(string.Empty,
                                "A Munkakör nem törölhető! " +
                                "A Munkakörre hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");

                // Back to Delete page with error message
                return View(jobDTO);
            }
            // update success
            return RedirectToAction(nameof(Index));

        }


        // ---------------------------- helper methods ----------------------------

        private void ConcurrencyHandler(DbUpdateConcurrencyException ex,
                                       ref JobDTO jobDTO)
        {
            var exceptionEntry = ex.Entries.Single();
            var clientValues = (Job)exceptionEntry.Entity;
            var databaseEntry = exceptionEntry.GetDatabaseValues();

            ModelState.Clear();

            if (databaseEntry == null)
            {
                ModelState.AddModelError(string.Empty,
                                "A módosításokat nem lehet menteni." +
                                "A Munkakört egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
            }
            else
            {
                var databaseValues = (Job)databaseEntry.ToObject();

                jobDTO.RowVersion = (byte[])databaseValues.RowVersion;
                ModelState.Remove("RowVersion");

                ModelState.AddModelError(string.Empty,
                                "A szerkesztés alatt a Munkakör adatait " +
                                "egy másik felhasználó megváltoztatta." +
                                "\nPirossal az adatbázisban jelenleg tárolt," +
                                " eltérő adatok láthatóak." +
                                "\nA [Mentés] gombra kattintva az adatbázis" +
                                " adatai felülíródnak." +
                                "\nA [Vissza] gombra kattintva módosításaid" +
                                " elvesznek.");

                if (databaseValues.JobName != clientValues.JobName)
                {
                    ModelState.AddModelError(nameof(databaseValues.JobName),
                        $"Adatbázisban: {databaseValues.JobName}");
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
                if (databaseValues.PreferOrder != clientValues.PreferOrder)
                {
                    ModelState.AddModelError(nameof(databaseValues.PreferOrder),
                        $"Adatbázisban: {databaseValues.PreferOrder}");
                }

            }
        }
    }
}
