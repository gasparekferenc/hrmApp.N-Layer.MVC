using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace hrmApp.Web.DTO
{
    public class ApplicationUserDTO : IdentityUser
    {
        [Display(Name = "Felhasználó neve")]
        public override string UserName { get; set; }

        [Display(Name = "Vezetéknév")]
        public string SurName { get; set; }

        [Display(Name = "Keresztnév")]
        public string ForeName { get; set; }

        [Display(Name = "GDPR irányelvek elfogadása")]
        public bool? GDPRConfirmed { get; set; }

        [Display(Name = "Használati feltételek elfogadása")]
        public bool? TermOfUseConfirmed { get; set; }

        public DateTime? DateOfPoliciesConfirm { get; set; }

        [Display(Name = "Megjegyzés")]
        public string Description { get; set; }
        public byte[] RowVersion { get; set; }

        public int? CurrentProjectId { get; set; }

        public ICollection<AssignmentDTO> Assignments { get; set; }

    }
}
