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
    public class DocTypeController : Controller
    {

        private readonly IDocTypeService _docTypeService;
        private readonly IMapper _mapper;

        public DocTypeController(IDocTypeService docTypeService, IMapper mapper)
        {
            _docTypeService = docTypeService;
            _mapper = mapper;
        }

        // GET: DocType --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var docTypes = await _docTypeService.GetAllAsync();
            var docTypeDTOs = _mapper.Map<IEnumerable<DocTypeDTO>>(docTypes);

            return View(docTypeDTOs);
        }

        // GET: DocType/Details/5 ----------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var docType = await _docTypeService.GetByIdAysnc((int)id);
            if (docType == null)
            {
                return NotFound();
            }
            var docTypeDTO = _mapper.Map<DocTypeDTO>(docType);
            return View(docTypeDTO);
        }

        // GET: DocType/Create -----------------------------------------------
        public IActionResult Create()
        {
            return View();
        }

        // POST: DocType/Create ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocTypeDTO docTypeDTO)
        {
            if (ModelState.IsValid)
            {
                var docType = _mapper.Map<DocType>(docTypeDTO);
                await _docTypeService.AddAsync(docType);
                return RedirectToAction(nameof(Index));
            }
            return View(docTypeDTO);
        }

        // GET: DocType/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var docType = await _docTypeService.GetByIdAysnc((int)id);
            if (docType == null)
            {
                return NotFound();
            }
            var docTypeDTO = _mapper.Map<DocTypeDTO>(docType);
            return View(docTypeDTO);
        }

        // POST: DocType/Edit/5 ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocTypeDTO docTypeDTO)
        {
            if (id != docTypeDTO.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var docType = _mapper.Map<DocType>(docTypeDTO);
                try
                {
                    await _docTypeService.UpdateAsync(docType);
                }
                // Database concurrency
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Information("Concurrency catching.", ex);

                    ConcurrencyHandler(ex, ref docTypeDTO);

                    // Database Conflict or Deleted record
                    // Back to Edit page with error message
                    return View(docTypeDTO);
                }
                // update success
                return RedirectToAction(nameof(Index));
            }
            // Model.State invalid
            return View(docTypeDTO);
        }


        // GET: DocType/Delete/5 -----------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var docType = await _docTypeService.GetWithDocumentsByIdAsync((int)id);
            if (docType == null)
            {
                return NotFound();
            }

            var docTypeDTO = _mapper.Map<DocTypeDTO>(docType);
            return View(docTypeDTO);
        }

        // POST: DocType/Delete/5 ----------------------------------------------
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, DocTypeDTO docTypeDTO)
        {
            if (id != docTypeDTO.Id)
            {
                return BadRequest();
            }

            var docType = await _docTypeService.GetWithDocumentsByIdAsync(id);

            if (docType == null)
            {
                //return NotFound();
                ModelState.AddModelError(string.Empty,
                                "A Dokumentum t??pust egy m??sik felhaszn??l?? t??r??lte." +
                                "\nKattints a [Vissza] gombra.");
                return View(docTypeDTO);
            }

            if (docType.Documents.Any())
            {
                Log.Information("There is related entity for this DocType.");
                ModelState.AddModelError(string.Empty,
                                "A Dokumentum t??pus nem t??r??lhet??! " +
                                "A Dokumentum t??pusra hivatkoz??sokat tartalmaz az adatb??zis." +
                                "\nKattints a [Vissza] gombra.");
                // Back to Delete page with error message
                return View(_mapper.Map<DocTypeDTO>(docType));
            }

            try
            {
                await _docTypeService.RemoveAsync(docType);
            }
            // Database constraint voliation
            catch (DbUpdateException ex)
            {
                Log.Information("Database constraint voliation.", ex);

                ModelState.Clear();
                ModelState.AddModelError(string.Empty,
                                "A Dokumentum t??pus nem t??r??lhet??! " +
                                "A Dokumentum t??pusra hivatkoz??sokat tartalmaz az adatb??zis." +
                                "\nKattints a [Vissza] gombra.");

                // Back to Delete page with error message
                return View(docTypeDTO);
            }
            // update success
            return RedirectToAction(nameof(Index));

        }


        // ---------------------------- helper methods ----------------------------

        private void ConcurrencyHandler(DbUpdateConcurrencyException ex,
                                       ref DocTypeDTO docTypeDTO)
        {
            var exceptionEntry = ex.Entries.Single();
            var clientValues = (DocType)exceptionEntry.Entity;
            var databaseEntry = exceptionEntry.GetDatabaseValues();

            ModelState.Clear();

            if (databaseEntry == null)
            {
                ModelState.AddModelError(string.Empty,
                                "A m??dos??t??sokat nem lehet menteni." +
                                "A Dokumentum t??pust egy m??sik felhaszn??l?? t??r??lte." +
                                "\nKattints a [Vissza] gombra.");
            }
            else
            {
                var databaseValues = (DocType)databaseEntry.ToObject();

                docTypeDTO.RowVersion = (byte[])databaseValues.RowVersion;
                ModelState.Remove("RowVersion");

                ModelState.AddModelError(string.Empty,
                                "A szerkeszt??s alatt a Dokumentum t??pus adatait " +
                                "egy m??sik felhaszn??l?? megv??ltoztatta." +
                                "\nPirossal az adatb??zisban jelenleg t??rolt," +
                                " elt??r?? adatok l??that??ak." +
                                "\nA [Ment??s] gombra kattintva az adatb??zis" +
                                " adatai fel??l??r??dnak." +
                                "\nA [Vissza] gombra kattintva m??dos??t??said" +
                                " elvesznek.");

                if (databaseValues.TypeName != clientValues.TypeName)
                {
                    ModelState.AddModelError(nameof(databaseValues.TypeName),
                        $"Adatb??zisban: {databaseValues.TypeName}");
                }
                if (databaseValues.Description != clientValues.Description)
                {
                    ModelState.AddModelError(nameof(databaseValues.Description),
                        $"Adatb??zisban: {databaseValues.Description}");
                }
                if (databaseValues.MandatoryElement != clientValues.MandatoryElement)
                {
                    ModelState.AddModelError(nameof(databaseValues.MandatoryElement),
                        $"Adatb??zisban: {databaseValues.MandatoryElement}");
                }
                if (databaseValues.IsActive != clientValues.IsActive)
                {
                    ModelState.AddModelError(nameof(databaseValues.IsActive),
                        $"Adatb??zisban: {databaseValues.IsActive}");
                }
                if (databaseValues.PreferOrder != clientValues.PreferOrder)
                {
                    ModelState.AddModelError(nameof(databaseValues.PreferOrder),
                        $"Adatb??zisban: {databaseValues.PreferOrder}");
                }

            }
        }
    }
}
