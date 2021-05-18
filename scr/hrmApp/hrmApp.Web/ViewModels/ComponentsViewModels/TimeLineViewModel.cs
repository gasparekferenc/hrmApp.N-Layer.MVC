using System;
using System.Collections.Generic;
using hrmApp.Web.DTO;

namespace hrmApp.Web.ViewModels.ComponentsViewModels
{
    public class TimeLineViewModel
    {
        public string Date { get; set; }
        public IEnumerable<HistoryDTO> Entries { get; set; }
    }
}