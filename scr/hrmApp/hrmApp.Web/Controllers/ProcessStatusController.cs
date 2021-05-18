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
    public class ProcessStatusController : Controller
    {

        private readonly IProcessStatusService _processStatusService;
        private readonly IMapper _mapper;

        public ProcessStatusController(IProcessStatusService processStatusService, IMapper mapper)
        {
            _processStatusService = processStatusService;
            _mapper = mapper;
        }

        // GET: ProcessStatus --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var processStatuss = await _processStatusService.GetAllAsync();
            var processStatusDTOs = _mapper.Map<IEnumerable<ProcessStatusDTO>>(processStatuss);

            return View(processStatusDTOs);
        }

        // GET: ProcessStatus/Details/5 ----------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var processStatus = await _processStatusService.GetByIdAysnc((int)id);
            if (processStatus == null)
            {
                return NotFound();
            }
            var processStatusDTO = _mapper.Map<ProcessStatusDTO>(processStatus);
            return View(processStatusDTO);
        }

        // GET: ProcessStatus/Create -----------------------------------------------
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessStatus/Create ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProcessStatusDTO processStatusDTO)
        {
            if (ModelState.IsValid)
            {
                var processStatus = _mapper.Map<ProcessStatus>(processStatusDTO);
                await _processStatusService.AddAsync(processStatus);
                return RedirectToAction(nameof(Index));
            }
            return View(processStatusDTO);
        }

        // GET: ProcessStatus/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var processStatus = await _processStatusService.GetByIdAysnc((int)id);
            if (processStatus == null)
            {
                return NotFound();
            }
            var processStatusDTO = _mapper.Map<ProcessStatusDTO>(processStatus);
            return View(processStatusDTO);
        }

        // POST: ProcessStatus/Edit/5 ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProcessStatusDTO processStatusDTO)
        {
            if (id != processStatusDTO.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var processStatus = _mapper.Map<ProcessStatus>(processStatusDTO);
                try
                {
                    await _processStatusService.UpdateAsync(processStatus);
                }
                // Database concurrency
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Information("Concurrency catching.", ex);

                    ConcurrencyHandler(ex, ref processStatusDTO);

                    // Database Conflict or Deleted record
                    // Back to Edit page with error message
                    return View(processStatusDTO);
                }
                // update success
                return RedirectToAction(nameof(Index));
            }
            // Model.State invalid
            return View(processStatusDTO);
        }


        // GET: ProcessStatus/Delete/5 -----------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var processStatus = await _processStatusService.GetWithEployeesByIdAsync((int)id);
            if (processStatus == null)
            {
                return NotFound();
            }

            var processStatusDTO = _mapper.Map<ProcessStatusDTO>(processStatus);
            return View(processStatusDTO);
        }

        // POST: ProcessStatus/Delete/5 ----------------------------------------------
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, ProcessStatusDTO processStatusDTO)
        {
            if (id != processStatusDTO.Id)
            {
                return BadRequest();
            }

            var processStatus = await _processStatusService.GetWithEployeesByIdAsync(id);

            if (processStatus == null)
            {
                //return NotFound();
                ModelState.AddModelError(string.Empty,
                                "A Munkavállaló státuszt egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
                return View(processStatusDTO);
            }

            if (processStatus.Employees.Any())
            {
                Log.Information("There is related entity for this ProcessStatus.");
                ModelState.AddModelError(string.Empty,
                                "A Munkavállaló státusz nem törölhető! " +
                                "A Munkavállaló státuszra hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");
                // Back to Delete page with error message
                return View(_mapper.Map<ProcessStatusDTO>(processStatus));
            }

            try
            {
                await _processStatusService.RemoveAsync(processStatus);
            }
            // Database constraint voliation
            catch (DbUpdateException ex)
            {
                Log.Information("Database constraint voliation.", ex);

                ModelState.Clear();
                ModelState.AddModelError(string.Empty,
                                "A Munkavállaló státusz nem törölhető! " +
                                "A Munkavállaló státuszra hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");

                // Back to Delete page with error message
                return View(processStatusDTO);
            }
            // update success
            return RedirectToAction(nameof(Index));

        }


        // ---------------------------- helper methods ----------------------------

        private void ConcurrencyHandler(DbUpdateConcurrencyException ex,
                                       ref ProcessStatusDTO processStatusDTO)
        {
            var exceptionEntry = ex.Entries.Single();
            var clientValues = (ProcessStatus)exceptionEntry.Entity;
            var databaseEntry = exceptionEntry.GetDatabaseValues();

            ModelState.Clear();

            if (databaseEntry == null)
            {
                ModelState.AddModelError(string.Empty,
                                "A módosításokat nem lehet menteni." +
                                "A Munkavállaló státuszt egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
            }
            else
            {
                var databaseValues = (ProcessStatus)databaseEntry.ToObject();

                processStatusDTO.RowVersion = (byte[])databaseValues.RowVersion;
                ModelState.Remove("RowVersion");

                ModelState.AddModelError(string.Empty,
                                "A szerkesztés alatt a Munkavállaló státusz adatait " +
                                "egy másik felhasználó megváltoztatta." +
                                "\nPirossal az adatbázisban jelenleg tárolt," +
                                " eltérő adatok láthatóak." +
                                "\nA [Mentés] gombra kattintva az adatbázis" +
                                " adatai felülíródnak." +
                                "\nA [Vissza] gombra kattintva módosításaid" +
                                " elvesznek.");

                if (databaseValues.StatusName != clientValues.StatusName)
                {
                    ModelState.AddModelError(nameof(databaseValues.StatusName),
                        $"Adatbázisban: {databaseValues.StatusName}");
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
