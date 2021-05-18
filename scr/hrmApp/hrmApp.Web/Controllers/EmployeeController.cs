using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AutoMapper;
using Serilog;
using hrmApp.Web.DTO;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.Constants;
using hrmApp.Web.ViewModels.ComponentsViewModels;
using hrmApp.Web.ViewModels.EmployeeViewModels;

namespace hrmApp.Web.Controllers
{

    [Authorize(Policy = PolicyNames.EmployeeHandler)]
    public class EmployeeController : Controller
    {
        #region EmployeeController : Controller
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

        public EmployeeController(
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

        private static string cr = Environment.NewLine;
        private string entry;
        private History historyEntry = new History();

        #endregion

        #region GET:  Employee/Index

        // GET: Employee/Index -----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                // ha User nincs bejelentkezve... (DE ide normál esetben nem kerül a vezérlés)
                //TODO: ReturnUrl?
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

            var employees = await _employeeService.GetEmployeesByUserIdAndProjectIdAysnc(userId, currentProjectId);

            EmployeeIndexViewModel viewModel = new EmployeeIndexViewModel
            {
                CurrentProjectId = currentProjectId,
                Employees = _mapper.Map<List<EmployeeDTO>>(employees)
            };

            return View(viewModel);
        }

        #endregion

        #region GET: Employee/Create

        // GET: Employee/Create -----------------------------------------------
        public async Task<IActionResult> Create()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                //TODO: ReturnUrl?
                return RedirectToAction(nameof(AccountController.Login), "Account", "/Employee/Create");
            }

            int currentProjectId = HttpContext.Session.GetInt32(SessionKeys.ProjectIdSessionKey) ?? 0;
            if (currentProjectId == 0)
            {
                // Ha a User-hez még nincs projekt rendelve
                throw new ArgumentException();
                //return View(new EmployeesViewModel { CurrentProjectId = 0 });
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var currentProjectName = (await _projectService.GetByIdAysnc(currentProjectId)).ProjectName;

            var organizations = await _organizationService.GetByUserIdandProjectIdAsync(userId, currentProjectId);
            var organizationsDTO = _mapper.Map<List<OrganizationDTO>>(organizations);

            var organizationSelectList = await AssembleOrganizationSelectList(userId, currentProjectId);
            var jobSelectList = await AssembleJobSelectList();
            var processStatusSelectList = await AssembleProcessStatusSelectList();

            NewEmployeeViewModel viewModel = new NewEmployeeViewModel
            {
                CurrentProjectId = currentProjectId,
                UserId = userId,
                SSNumber = "",
                Birthplace = "",
                DateOfBirth = DateTime.Now,             
                Organizations = organizationSelectList,
                SelectedOrganizationId = 1,             
                Jobs = jobSelectList,
                SelectedJobId = Int32.Parse(jobSelectList.FirstOrDefault().Value),                      
                ProcessStatuses = processStatusSelectList,
                SelectedProcessStatusId = Int32.Parse(processStatusSelectList.FirstOrDefault().Value)   
            };

            return View(viewModel);
        }
        #endregion

        #region POST: Employee/Create

        // POST: Employee/Create ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewEmployeeViewModel viewModel)
        {
            POEmployee pOEmployee = new POEmployee();
            Employee employee = new Employee();

            if (ModelState.IsValid)
            {
                var sSNumber = viewModel.SSNumber;
                var userId = viewModel.UserId;
                var projectId = viewModel.CurrentProjectId;
                var organizationId = viewModel.SelectedOrganizationId;
                var projectOrganization = await _projectOrganizationService.GetByProjectIdAndOrganizationIdAsync(projectId, organizationId);

                if (await _employeeService.ExistSSNumber(sSNumber)) // A TAJ szám már létezik a rendszerben.
                {
                    employee = await _employeeService.GetBySSNumber(sSNumber);

                    if (employee.IsActive)  // Hibaüzenet visszaküldése
                    {
                        // TODO: További infók Employee-ról...
                        ModelState.AddModelError(string.Empty, "A TAJ számhoz tartozó munkavállaló aktív a rendszervben!");

                        viewModel.Organizations = await AssembleOrganizationSelectList(
                                                        viewModel.UserId, viewModel.CurrentProjectId);
                        viewModel.Jobs = await AssembleJobSelectList();
                        viewModel.ProcessStatuses = await AssembleProcessStatusSelectList();
                        return View(viewModel);
                    }
                    else    // Adatok átvétele és Employee/DataSheet
                    {
                        // bejegyezés az POEmployees táblába
                        pOEmployee = new POEmployee
                        {
                            ProjectOrganizationId = projectOrganization.Id,
                            EmployeeId = employee.Id
                        };
                        await _pOEmployeeService.AddAsync(pOEmployee);

                        // Az adatokat felülírjuk vagy tatartsuk meg?                        
                        // employee.SurName = viewModel.SurName;
                        // employee.ForeName = viewModel.ForeName;
                        // employee.Birthplace = viewModel.Birthplace;
                        // employee.DateOfBirth = viewModel.DateOfBirth;
                        // employee.SSNumber = viewModel.SSNumber;
                        employee.JobId = viewModel.SelectedJobId;
                        employee.ProcessStatusId = viewModel.SelectedProcessStatusId;
                        employee.IsActive = true;

                        await _employeeService.UpdateAsync(employee);
                        employee = await _employeeService.GetWithIncludesByIdAsync(employee.Id);

                        // Add entry to Histories (TimeLine)
                        entry = $"{employee.FullName}({employee.SSNumber}) {cr}" +
                                    $"{employee.Birthplace}, {employee.DateOfBirth}{cr}" +
                                    $"{employee.Job.JobName}, {employee.ProcessStatus.StatusName}{cr}" +
                                    $"{cr}";

                        historyEntry = await _historyService.AddEntry(entry, EntryTypes.ReEnrollment, employee.Id, userId);

                        return RedirectToAction(nameof(Index));
                    }
                }
                // Ha nincs a rendszerben, felvesszük ...
                employee = new Employee
                {
                    SurName = viewModel.SurName,
                    ForeName = viewModel.ForeName,
                    Birthplace = viewModel.Birthplace,
                    DateOfBirth = viewModel.DateOfBirth,
                    SSNumber = viewModel.SSNumber,
                    JobId = viewModel.SelectedJobId,
                    ProcessStatusId = viewModel.SelectedProcessStatusId,
                    IsActive = true
                };
                var newEmployee = await _employeeService.AddAsync(employee);
                // ... és bejegyezzük a POEmployee táblába
                pOEmployee = new POEmployee
                {
                    ProjectOrganizationId = projectOrganization.Id,
                    EmployeeId = newEmployee.Id
                };
                await _pOEmployeeService.AddAsync(pOEmployee);
                newEmployee = await _employeeService.GetWithIncludesByIdAsync(newEmployee.Id);

                // Add entry to Histories (TimeLine)
                entry = $"{newEmployee.FullName}({newEmployee.SSNumber}) {cr}" +
                            $"{newEmployee.Birthplace}, {newEmployee.DateOfBirth}{cr}" +
                            $"{newEmployee.Job.JobName}, {newEmployee.ProcessStatus.StatusName}{cr}" +
                            $"{cr}";

                historyEntry = await _historyService.AddEntry(entry, EntryTypes.Create, newEmployee.Id, userId);

                Log.Information($"New Employee (Id={newEmployee.Id}) added.");

                // TODO: Toast??
                return RedirectToAction(nameof(Index));
            }

            // !ModelState.IsValid
            ModelState.AddModelError(string.Empty, "Hiba az adatokban");

            viewModel.Organizations = await AssembleOrganizationSelectList(
                                            viewModel.UserId, viewModel.CurrentProjectId);
            viewModel.Jobs = await AssembleJobSelectList();
            viewModel.ProcessStatuses = await AssembleProcessStatusSelectList();

            return View(viewModel);
        }

        #endregion

        #region GET: Employee/Details/5
        // GET:  Employee/Details/5 --------------------------------------------------------
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Details(int id)
        public IActionResult Details(int id)
        {
            Log.Information("TODO: Details Implementation...");
            return RedirectToAction(nameof(Index));         
        }
        #endregion

        #region POST: Employee/Delete/5
        // POST:  Employee/Delete/5 --------------------------------------------------------
        //[HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Delete(int id)
        public IActionResult Delete(int id)
        {
            Log.Information("TODO: Delete(int Id) Implementation...");
            return RedirectToAction(nameof(Index));            
        }
        #endregion

        #region GET:  Employee/DataSheet/5
        // GET:  Employee/DataSheet/5 --------------------------------------------------------
        public async Task<IActionResult> DataSheet(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
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

            HttpContext.Session.SetInt32(SessionKeys.EmployeeIdSessionKey, id);
            var employee = await _employeeService.GetByIdAysnc(id);

            var viewModel = new DataSheetViewModel
            {
                CurrentProjectId = currentProjectId,
                UserId = userId,
                Employee = _mapper.Map<EmployeeDTO>(employee)
            };

            // Itt kellene elleőrizni a User jogosultságot. Ide menüből nem juthat..., de bemásolhat url-t...
            var employeeOrganization = await _organizationService.GetByEmployeeIdAsync(employee.Id);
            var assignment = (await _organizationService.GetWithAssignmentsByIdAsync(employeeOrganization.Id))
                                .Assignments
                                .SingleOrDefault(u => u.ApplicationUserId == userId);
            if (assignment == null){
                Log.Information($"DataSheet access denied.");
                throw new System.Exception("Nincs hozzáférési jogosultságod ennek a tartalomnak az eléréséhez!");
            }


            Log.Information($"DataSheet loaded.");
            return View(viewModel);
        }
        #endregion

        #region POST: Employee/ChangeEmployeeData
        // POST:  Employee/ChangeEmployeeData --------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmployeeData(DataSheetEmployeeViewModel viewModel)
        {
            var employeeId = viewModel.EmployeeId;
            var employee = await _employeeService.GetByIdAysnc(employeeId);

            EmployeeDTO employeeOriginal = _mapper.Map<EmployeeDTO>(employee);

            _mapper.Map(viewModel.Employee, employee);
            employee.JobId = viewModel.SelectedJobId;

            try
            {
                await _employeeService.UpdateAsync(employee);
                TempData["ChangeEmployeeDataMessage"] = BaseValues.ChangeEmployeeDataSuccess;
            }
            catch (Exception ex)
            {
                TempData["ChangeEmployeeDataMessage"] = BaseValues.ChangeEmployeeDataFailed;
                Log.Information($"ChangeEmployeeData failed! Employee.Id={employeeId}. Error: {ex}");
                //throw;
            }

            // Add entry to Histories (TimeLine)
            EmployeeDTO employeeNew = _mapper.Map<EmployeeDTO>(employee);
            entry = await GetDifferece(employeeOriginal, employeeNew);

            historyEntry = await _historyService.AddEntry(entry, EntryTypes.Update, employee.Id, viewModel.UserId);

            return RedirectToAction(nameof(DataSheet), new { id = employeeId });
        }
        #endregion

        #region POST: Employee/ChangeProcessStatus
        // POST:  Employee/ChangeProcessStatus --------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProcessStatus(ProcessStatusViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            var userId = user.Id;

            var employeeId = viewModel.EmployeeId;
            var processStatusId = viewModel.SelectedProcessStatusId;
            var newProcessStatusName = (await _processStatusService.GetByIdAysnc(processStatusId)).StatusName;

            var employee = await _employeeService.GetByIdAysnc(employeeId);
            var oriProcessStatusName = (await _processStatusService.GetByIdAysnc(employee.ProcessStatusId)).StatusName;
            employee.ProcessStatusId = processStatusId;

            try
            {
                await _employeeService.UpdateAsync(employee);
                TempData["ChangeProcessStatusMessage"] = BaseValues.ChangeProcessStatusSuccess;
            }
            catch (Exception ex)
            {
                TempData["ChangeProcessStatusMessage"] = BaseValues.ChangeProcessStatusFailed;
                Log.Information($"ChangeProcessStatus change failed! Employee.Id={employeeId}. Error: {ex}");
                //throw;
            }
            // Add entry to Histories (TimeLine)
            entry = $"Felvételi folyamat státusz változás: {oriProcessStatusName} => {newProcessStatusName}";
            historyEntry = await _historyService.AddEntry(entry, EntryTypes.StatusChange, employee.Id, userId);

            return RedirectToAction(nameof(DataSheet), new { id = employeeId });
        }
        #endregion

        #region POST: Employee/ChangeOrganization
        // POST:  Employee/ChangeOrganization --------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeOrganization(OrganizationViewModel viewModel)
        {
            int currentProjectId = HttpContext.Session.GetInt32(SessionKeys.ProjectIdSessionKey) ?? 0;
            if (currentProjectId == 0)
            {
                throw new ArgumentException();
            }

            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            var userId = user.Id;

            var employeeId = viewModel.EmployeeId;
            var newOrganizationId = viewModel.SelectedOrganizationId;
            var newOrganizationName = (await _organizationService.GetByIdAysnc(newOrganizationId)).OrganizationName;

            var oriOrganization = await _organizationService.GetByEmployeeIdAsync(employeeId);
            var oriOrganizationName = (await _organizationService.GetByEmployeeIdAsync(employeeId)).OrganizationName;

            var newProjectOrganization = await _projectOrganizationService.GetByProjectIdAndOrganizationIdAsync(currentProjectId, newOrganizationId);

            var pOEmployee = (await _pOEmployeeService.GetAllByProjectIdAsync(currentProjectId))
                                                        .Where(poe => poe.EmployeeId == employeeId)
                                                        .SingleOrDefault();

            pOEmployee.ProjectOrganizationId = newProjectOrganization.Id;

            try
            {
                await _pOEmployeeService.UpdateAsync(pOEmployee);

                TempData["ChangeOrganizationMessage"] = BaseValues.ChangeOrganizationSuccess;
            }
            catch (Exception ex)
            {
                TempData["ChangeOrganizationMessage"] = BaseValues.ChangeOrganizationFailed;
                Log.Information($"ChangeOrganization change failed! Employee.Id={employeeId}. Error: {ex}");
                //throw;
            }
            // Add entry to Histories (TimeLine)
            entry = $"Szervezeti elhelyezés változás: {oriOrganizationName} => {newOrganizationName}";
            historyEntry = await _historyService.AddEntry(entry, EntryTypes.OrganizationChange, employeeId, userId);

            return RedirectToAction(nameof(DataSheet), new { id = employeeId });
        }
        #endregion

        #region POST: Employee/CreateMemo
        // POST:  Employee/CreateMemo --------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMemo(MemoViewModel viewModel)
        {
            int currentProjectId = HttpContext.Session.GetInt32(SessionKeys.ProjectIdSessionKey) ?? 0;
            if (currentProjectId == 0)
            {
                // Ha a User-hez még nincs projekt rendelve
                throw new ArgumentException();
                //return View(new EmployeesViewModel { CurrentProjectId = 0 });
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
                var userId = user.Id;

                var employeeId = viewModel.EmployeeId;
                try
                {
                    historyEntry = await _historyService.AddEntry(
                                                        entry: viewModel.Entry,
                                                        entryType: viewModel.IsReminder ? EntryTypes.MemoWithReminder : EntryTypes.Memo,
                                                        employeeId: employeeId,
                                                        userId: userId,
                                                        documentId: null,
                                                        isRemainder: viewModel.IsReminder,
                                                        deadLineDate: viewModel.DeadlineDate
                                                        );
                    TempData["CreateMemoMessage"] = BaseValues.CreateMemoSuccess;
                    Log.Information($"CreateMemo success! Employee.Id={employeeId}.");
                }
                catch (Exception ex)
                {
                    TempData["CreateMemoMessage"] = BaseValues.CreateMemoFailed;
                    Log.Information($"CreateMemo failed! Employee.Id={employeeId}. Error: {ex}");
                    //throw;
                }

                return RedirectToAction(nameof(DataSheet), new { id = employeeId });
            }

            // !ModelState.IsValid
            ModelState.AddModelError(string.Empty, "Hiba az adatokban");

            return View(viewModel);
        }

        #endregion

        #region POST: Employee/UploadFile

        // POST: Employee/UploadFile --------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(DocumentsViewModel viewModel)
        {
            var newDocument = new Document();

            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            var userId = user.Id;

            var employeeId = viewModel.EmployeeId;
            var file = viewModel.AttachedFile;

            if (ModelState.IsValid)
            {
                var employeePath = employeeId.ToString("00000000");
                string fileRelativePath;
                try
                {
                    fileRelativePath = await _fileService.SaveFileAsync(file, employeePath);
                }
                catch (Exception ex)
                {
                    Log.Information($"File upload failed!. Error: {ex}");
                    TempData["DocumentsMessage"] = BaseValues.FileUploadFailed;
                    return RedirectToAction(nameof(DataSheet), new { id = employeeId });
                    // throw;
                }

                var employeDocument = new Document
                {
                    DocDisplayName = viewModel.AttachedFile.FileName,
                    DocPath = fileRelativePath,
                    UploadedTimeStamp = DateTime.Now,
                    Description = viewModel.Description,
                    EmployeeId = employeeId,
                    DocTypeId = viewModel.SelectedDocTypeId
                };
                try
                {
                    newDocument = await _documentService.AddAsync(employeDocument);
                }
                catch (Exception ex)
                {
                    Log.Information($"File upload failed!. Error: {ex}");
                    TempData["DocumentsMessage"] = BaseValues.FileUploadFailed;
                    return RedirectToAction(nameof(DataSheet), new { id = employeeId });
                    // throw;
                }

                var documentTypeName = (await _docTypeService.GetByIdAysnc(viewModel.SelectedDocTypeId)).TypeName;

                Log.Information("File upload success.");
                TempData["DocumentsMessage"] = BaseValues.FileUploadSuccess;

                // Add entry to Histories (TimeLine)
                entry = $"Dokumentum feltöltése - {viewModel.AttachedFile.FileName} ({documentTypeName})";
                historyEntry = await _historyService.AddEntry(entry, EntryTypes.UploadDocument, employeeId, userId, newDocument.Id);

                return RedirectToAction(nameof(DataSheet), new { id = employeeId });
            }

            // !ModelState.IsValid 
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            // TODO: Errors send Back to UI...

            Log.Information("File upload failed...!");
            TempData["DocumentsMessage"] = BaseValues.FileUploadFailed;

            return RedirectToAction(nameof(DataSheet), new { id = employeeId });
        }

        #endregion

        #region POST: Employee/DeleteFile/5
        // POST: Employee/DeleteFile --------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFile(int id, int employeeId)
        {
            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            var userId = user.Id;

            var documentToDelete = await _documentService.GetByIdAysnc(id);
            if (documentToDelete == null)
            {
                Log.Information($"File deleted failed...! (Document.Id={id})");
                TempData["DocumentsMessage"] = BaseValues.FileDeleteFailed;
                return RedirectToAction(nameof(DataSheet), new { id = employeeId });
            }
            var documentPath = documentToDelete.DocPath;


            try
            {
                await _documentService.RemoveAsync(documentToDelete);
                var fileDeleted = _fileService.DeleteFile(documentPath);
                if (fileDeleted)
                {
                    Log.Information($"File susseccful deleted! {documentPath}");
                    TempData["DocumentsMessage"] = BaseValues.FileDeleteSuccess;

                    // Add entry to Histories (TimeLine)
                    entry = $"Dokumentum törlése - {documentToDelete.DocDisplayName}";
                    historyEntry = await _historyService.AddEntry(entry, EntryTypes.DeleteDocument, employeeId, userId);

                    return RedirectToAction(nameof(DataSheet), new { id = employeeId });
                }
                Log.Information($"File delete failed! {documentPath}");
                TempData["DocumentsMessage"] = BaseValues.FileDeleteFailed;
                return RedirectToAction(nameof(DataSheet), new { id = employeeId });
            }
            catch (Exception ex)
            {
                Log.Information($"File delete failed...! {ex}");
                TempData["DocumentsMessage"] = BaseValues.FileDeleteFailed;
                return RedirectToAction(nameof(DataSheet), new { id = employeeId });
                // throw;
            }

        }
        #endregion

        #region Get: Download/5
        // Get: Employee/Download --------------------------------------------------------
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Download(int id, int employeeId)
        {

            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            var userId = user.Id;

            var documentToDownload = await _documentService.GetByIdAysnc(id);
            if (documentToDownload == null)
            {
                Log.Information($"File to download failed...! (Document.Id={id})");
                TempData["DocumentsMessage"] = BaseValues.FileDownloadFailed;
                return RedirectToAction(nameof(DataSheet), new { id = employeeId });
            }
            var documentPath = documentToDownload.DocPath;

            if (!string.IsNullOrEmpty(documentPath))
            {
                var fullPath = _fileService.GetFullPath(documentPath);

                if (System.IO.File.Exists(fullPath))
                {
                    //var downloadFileName = $"{Guid.NewGuid()}.pdf";
                    var downloadFileName = _fileService.ConvertValidFileName(documentToDownload.DocDisplayName);
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    Log.Information("File downloaded successful.");
                    TempData["DocumentsMessage"] = BaseValues.FileDownloadSuccess;

                    // Add entry to Histories (TimeLine)
                    entry = $"Dokumentum letöltése - {documentToDownload.DocDisplayName}";
                    historyEntry = await _historyService.AddEntry(entry, EntryTypes.DownloadDocument, employeeId, userId);

                    memory.Position = 0;
                    return File(memory, "application/pdf", downloadFileName);
                }
            }
            
            Log.Information($"File to download failed! 'documentPath' Null or Empty (Document.Id={id})");
            TempData["DocumentsMessage"] = BaseValues.FileDownloadFailed;
            return RedirectToAction(nameof(DataSheet), new { id = employeeId });
        }

        #endregion

        #region Helpers

        #region GetDifferece(employeeOriginal, employee)
        private async Task<string> GetDifferece(EmployeeDTO employeeOriginal, EmployeeDTO employeeNew)
        {
            object value1, value2;
            string result = "";
            string[] skipProperties = new string[]{
                "Id", "FullName", "ProcessStatus", "Job", "Histories",
                "Documents", "POEmployee", "RowVersion"
            };

            PropertyInfo[] properties = employeeOriginal.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!skipProperties.Contains(propertyInfo.Name))
                {
                    value1 = propertyInfo.GetValue(employeeOriginal, null);
                    value2 = propertyInfo.GetValue(employeeNew, null);

                    if (!ValuesEquals(value1, value2))
                    {
                        if (propertyInfo.Name.Equals(nameof(EmployeeDTO.JobId)))
                        {
                            var DisplayNameAttribute = typeof(JobDTO)
                                                    .GetProperty(nameof(JobDTO.JobName))
                                                    .GetCustomAttribute<DisplayAttribute>()?
                                                    .Name;
                            var jobs = await _jobService.GetAllAsync();
                            var jobName1 = jobs.SingleOrDefault(job => job.Id == (int)value1).JobName;
                            var jobName2 = jobs.SingleOrDefault(job => job.Id == (int)value2).JobName;
                            result = result + ($"{DisplayNameAttribute}: {jobName1} => {jobName2} {cr}");
                        }
                        else
                        {
                            var DisplayNameAttribute = typeof(EmployeeDTO)
                                                    .GetProperty(propertyInfo.Name)
                                                    .GetCustomAttribute<DisplayAttribute>()?
                                                    .Name;
                            if (propertyInfo.GetType().Equals(typeof(DateTime)))
                            {
                                var datum1 = (DateTime)value1;
                                var datum2 = (DateTime)value2;
                                result = result + ($"{DisplayNameAttribute}: {datum1.ToString("yyyy.MM.dd.")} => {datum2.ToString("yyyy.MM.dd.")} {cr}");
                            }
                            else
                            {
                                result = result + ($"{DisplayNameAttribute}: {value1} => {value2} {cr}");
                            }
                        }
                    }
                }
            }

            return result;
        }
        #region ValuesEquals(object value1, object value2)
        private static bool ValuesEquals(object o1, object o2)
        {
            bool valuesEqual = true;
            IComparable selfValueComparer = o1 as IComparable;

            // one of the values is null            
            if (o1 == null && o2 != null || o1 != null && o2 == null)
                valuesEqual = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(o2) != 0)
                valuesEqual = false;
            else if (!object.Equals(o1, o2))
                valuesEqual = false;

            return valuesEqual;
        }
        #endregion

        #endregion

        #region AssembleOrganizationSelectList(userId, projectId)

        private async Task<SelectList> AssembleOrganizationSelectList(string userId, int projectId)
        {
            var organizations = await _organizationService.GetByUserIdandProjectIdAsync(userId, projectId);
            var organizationsDTO = _mapper.Map<List<OrganizationDTO>>(organizations);
            var organizationSelectList = new SelectList(
                                        organizationsDTO,
                                        nameof(OrganizationDTO.Id),
                                        nameof(OrganizationDTO.OrganizationName));
            return organizationSelectList;
        }
        #endregion

        #region AssembleJobSelectList()

        private async Task<SelectList> AssembleJobSelectList()
        {
            var jobs = (await _jobService.GetAllAsync()).OrderBy(j => j.PreferOrder);
            var jobDTO = _mapper.Map<List<JobDTO>>(jobs);
            var jobSelectList = new SelectList(
                                        jobDTO,
                                        nameof(JobDTO.Id),
                                        nameof(JobDTO.JobName));
            return jobSelectList;
        }
        #endregion

        #region AssembleProcessStatusSelectList()

        private async Task<SelectList> AssembleProcessStatusSelectList()
        {
            var statusprocessStatuses = (await _processStatusService.GetAllAsync()).OrderBy(s => s.PreferOrder);
            var statusprocessStatusesDTO = _mapper.Map<List<ProcessStatusDTO>>(statusprocessStatuses);
            var processStatusSelectList = new SelectList(
                                        statusprocessStatusesDTO,
                                        nameof(ProcessStatusDTO.Id),
                                        nameof(ProcessStatusDTO.StatusName));
            return processStatusSelectList;
        }
        #endregion

        #endregion
    }
}