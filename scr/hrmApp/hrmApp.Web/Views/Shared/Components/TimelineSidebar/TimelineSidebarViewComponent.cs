using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using hrmApp.Core.Services;
using hrmApp.Web.DTO;
using hrmApp.Web.ViewModels.ComponentsViewModels;

namespace hrmApp.Web.Views.Shared.Components.TimelineSidebar
{
    public class TimelineSidebarViewComponent : ViewComponent
    {
        private readonly IHistoryService _historyService;
        private readonly IMapper _mapper;
        public TimelineSidebarViewComponent(
            IHistoryService historyService,
            IMapper mapper
        )
        {
            _historyService = historyService;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? employeeId)
        {
            var viewModel = new List<TimeLineViewModel>();

            if (employeeId != null)
            {
                var entries = await _historyService.GetAllByEmployeeIdAsync((int)employeeId);

                viewModel = entries.GroupBy(e => e.EntryDate.ToString("yyyy.MM.dd"))
                                .Select(group => new TimeLineViewModel
                                {
                                    Date = group.Key,
                                    Entries = _mapper.Map<List<HistoryDTO>>(group.ToList())
                                }).OrderBy(r => r.Date)
                                .ToList();
            }
            return View(viewModel);
        }
    }
}