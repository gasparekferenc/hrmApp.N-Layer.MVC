using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using hrmApp.Web.Constants;
using hrmApp.Web.DTO;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.ViewModels.ComponentsViewModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hrmApp.Web.Views.Shared.Components.DataSheetProcessStatus
{
    public class DataSheetProcessStatusViewComponent : ViewComponent
    {
        private readonly IEmployeeService _employeeService;
        private readonly IProcessStatusService _processStatusService;
        private readonly IMapper _mapper;

        public DataSheetProcessStatusViewComponent(
            IEmployeeService employeeService,
            IProcessStatusService processStatusService,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _processStatusService = processStatusService;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(int employeeId)
        {
            // Hozzáférés ellenőrzés (UserID)?

            var message = (string)TempData["ChangeProcessStatusMessage"] ?? "";

            var employee = await _employeeService.GetByIdAysnc(employeeId);
            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            var processStatusId = employeeDTO.ProcessStatusId;

            var processStatuses = (await _processStatusService.GetAllAsync())
                                    .OrderBy(s => s.PreferOrder)
                                    .ToList();
            var prcessStatusesDTO = _mapper.Map<IEnumerable<ProcessStatusDTO>>(processStatuses);

            var viewModel = new ProcessStatusViewModel
            {
                EmployeeId = employeeId,
                Message = message,
                ProcessStatuses = new SelectList(
                                        prcessStatusesDTO,
                                        nameof(ProcessStatusDTO.Id),
                                        nameof(ProcessStatusDTO.StatusName)
                ),
                SelectedProcessStatusId = processStatusId
            };

            return View(viewModel);
        }
    }
}