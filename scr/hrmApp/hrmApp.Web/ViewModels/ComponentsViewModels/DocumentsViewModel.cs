using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hrmApp.Web.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hrmApp.Web.ViewModels.ComponentsViewModels
{
    public class DocumentsViewModel
    {
        public int EmployeeId { get; set; }
        public string Message { get; set; }

        [Required(ErrorMessage = "Válassz ki egy feltöltendő állományt.")]
        public IFormFile AttachedFile { get; set; }
        public IEnumerable<SelectListItem> DocTypes { get; set; }
        public int SelectedDocTypeId { get; set; }

        [Display(Name = "Leírás")]
        public string Description { get; set; }

        public IEnumerable<DocumentDTO> Documents { get; set; }
    }
}
