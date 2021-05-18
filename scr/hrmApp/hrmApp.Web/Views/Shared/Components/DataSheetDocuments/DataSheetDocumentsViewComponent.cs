using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using hrmApp.Web.DTO;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.ViewModels.ComponentsViewModels;

namespace hrmApp.Web.Views.Shared.Components.DataSheetDocuments
{
    public class DataSheetDocumentsViewComponent : ViewComponent
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDocumentService _documentService;
        private readonly IDocTypeService _docTypeService;
        private readonly IMapper _mapper;

        public DataSheetDocumentsViewComponent(
            IEmployeeService employeeService,
            IDocumentService documentService,
            IDocTypeService docTypeService,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _documentService = documentService;
            _docTypeService = docTypeService;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(int employeeId)
        {

            var message = (string)TempData["DocumentsMessage"] ?? "";

            var employee = await _employeeService.GetByIdAysnc(employeeId);
            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);

            var docTypes = (await _docTypeService.GetAllAsync()).OrderBy(s => s.PreferOrder);

            var docTypesDTO = _mapper.Map<IEnumerable<DocTypeDTO>>(docTypes);

            var documents = (await _documentService.GetAllByEmployeeIdAsync(employeeId))
                                                            .OrderBy(d => d.UploadedTimeStamp)
                                                            .ToList();
            var documentsDTO = _mapper.Map<IEnumerable<DocumentDTO>>(documents);

            var viewModel = new DocumentsViewModel
            {
                EmployeeId = employeeId,

                Message = message,
                DocTypes = new SelectList(
                                        docTypesDTO,
                                        nameof(DocTypeDTO.Id),
                                        nameof(DocTypeDTO.TypeName)
                ),
                SelectedDocTypeId = -1,   // -1 a selectList-nek az 'Típus választás...' eleme!
                Description = "",
                Documents = documentsDTO
            };

            return View(viewModel);
        }
    }
}