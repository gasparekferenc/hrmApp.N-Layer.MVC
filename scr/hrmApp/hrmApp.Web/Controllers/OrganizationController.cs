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

namespace hrmApp
{
    [Authorize(Policy = PolicyNames.BaseDataHandler)]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly IMapper _mapper;

        public OrganizationController(IOrganizationService organizationService, IMapper mapper)
        {
            _organizationService = organizationService;
            _mapper = mapper;
        }

        // GET: Organization --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var organizations = await _organizationService.GetAllAsync();
            var organizationDTOs = _mapper.Map<IEnumerable<OrganizationDTO>>(organizations);

            return View(organizationDTOs);
        }

        // GET: Organization/Details/5 ----------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var organization = await _organizationService.GetByIdAysnc((int)id);
            if (organization == null)
            {
                return NotFound();
            }
            var organizationDTO = _mapper.Map<OrganizationDTO>(organization);
            return View(organizationDTO);
        }

        // GET: Organization/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organization/Create ------------------------------------------------
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationDTO organizationDTO)
        {
            if (ModelState.IsValid)
            {
                var organization = _mapper.Map<Organization>(organizationDTO);
                await _organizationService.AddAsync(organization);
                return RedirectToAction(nameof(Index));
            }
            return View(organizationDTO);
        }

        // GET: Organization/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var organization = await _organizationService.GetByIdAysnc((int)id);
            if (organization == null)
            {
                return NotFound();
            }
            var organizationDTO = _mapper.Map<OrganizationDTO>(organization);
            return View(organizationDTO);
        }

        // POST: Organization/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrganizationDTO organizationDTO)
        {
            if (id != organizationDTO.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var organization = _mapper.Map<Organization>(organizationDTO);
                try
                {
                    await _organizationService.UpdateAsync(organization);
                }
                // Database concurrency
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Information("Concurrency catching.", ex);

                    ConcurrencyHandler(ex, ref organizationDTO);

                    // Database Conflict or Deleted record
                    // Back to Edit page with error message
                    return View(organizationDTO);
                }
                // update success
                return RedirectToAction(nameof(Index));
            }
            // Model.State invalid
            return View(organizationDTO);
        }

        // GET: Organization/Delete/5 -----------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var organization = await _organizationService.GetWithPOByIdAsync((int)id);
            if (organization == null)
            {
                return NotFound();
            }

            var organizationDTO = _mapper.Map<OrganizationDTO>(organization);
            return View(organizationDTO);
        }

        // POST: Organization/Delete/5 ----------------------------------------------
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, OrganizationDTO organizationDTO)
        {
            if (id != organizationDTO.Id)
            {
                return BadRequest();
            }

            var organization = await _organizationService.GetWithPOByIdAsync(id);

            if (organization == null)
            {
                //return NotFound();
                ModelState.AddModelError(string.Empty,
                                "A Szervezetet egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
                return View(organizationDTO);
            }

            if (organization.ProjectOrganizations.Any())
            {
                Log.Information("There is related entity.");
                ModelState.AddModelError(string.Empty,
                                "A Szervezet nem törölhető! " +
                                "A Szervezetre hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");
                // Back to Delete page with error message
                return View(_mapper.Map<OrganizationDTO>(organization));
            }

            try
            {
                await _organizationService.RemoveAsync(organization);
            }
            // Database constraint voliation
            catch (DbUpdateException ex)
            {
                Log.Information("Database constraint voliation.", ex);

                ModelState.Clear();
                ModelState.AddModelError(string.Empty,
                                "A Szervezet nem törölhető! " +
                                "A Szervezetre hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");

                // Back to Delete page with error message
                return View(organizationDTO);
            }
            // update success
            return RedirectToAction(nameof(Index));

        }

        // ---------------------------- helper methods ----------------------------

        private void ConcurrencyHandler(DbUpdateConcurrencyException ex,
                                       ref OrganizationDTO organizationDTO)
        {
            var exceptionEntry = ex.Entries.Single();
            var clientValues = (Organization)exceptionEntry.Entity;
            var databaseEntry = exceptionEntry.GetDatabaseValues();

            ModelState.Clear();

            if (databaseEntry == null)
            {
                ModelState.AddModelError(string.Empty,
                                "A módosításokat nem lehet menteni." +
                                "A Szervezetet egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
            }
            else
            {
                var databaseValues = (Organization)databaseEntry.ToObject();

                organizationDTO.RowVersion = (byte[])databaseValues.RowVersion;
                ModelState.Remove("RowVersion");

                ModelState.AddModelError(string.Empty,
                                "A szerkesztés alatt a Szervezet adatait " +
                                "egy másik felhasználó megváltoztatta." +
                                "\nPirossal az adatbázisban jelenleg tárolt," +
                                " eltérő adatok láthatóak." +
                                "\nA [Mentés] gombra kattintva az adatbázis" +
                                " adatai felülíródnak." +
                                "\nA [Vissza] gombra kattintva módosításaid" +
                                " elvesznek.");
                if (databaseValues.OrganizationName != clientValues.OrganizationName)
                {
                    ModelState.AddModelError(nameof(databaseValues.OrganizationName),
                        $"Adatbázisban: {databaseValues.OrganizationName}");
                }
                if (databaseValues.City != clientValues.City)
                {
                    ModelState.AddModelError(nameof(databaseValues.City),
                        $"Adatbázisban: {databaseValues.City}");
                }
                if (databaseValues.Address != clientValues.Address)
                {
                    ModelState.AddModelError(nameof(databaseValues.Address),
                        $"Adatbázisban: {databaseValues.Address}");
                }
                if (databaseValues.ContactName != clientValues.ContactName)
                {
                    ModelState.AddModelError(nameof(databaseValues.ContactName),
                        $"Adatbázisban: {databaseValues.ContactName}");
                }
                if (databaseValues.ContactEmail != clientValues.ContactEmail)
                {
                    ModelState.AddModelError(nameof(databaseValues.ContactEmail),
                        $"Adatbázisban: {databaseValues.ContactEmail}");
                }
                if (databaseValues.ContactPhone != clientValues.ContactPhone)
                {
                    ModelState.AddModelError(nameof(databaseValues.ContactPhone),
                        $"Adatbázisban: {databaseValues.ContactPhone}");
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
