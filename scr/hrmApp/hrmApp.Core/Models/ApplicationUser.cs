using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace hrmApp.Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string SurName { get; set; }
        public string ForeName { get; set; }
        public bool? GDPRConfirmed { get; set; }
        public bool? TermOfUseConfirmed { get; set; }
        public DateTime? DateOfPoliciesConfirm { get; set; }
        public string Description { get; set; }
        public byte[] RowVersion { get; set; }
        public int? CurrentProjectId { get; set; }

        // Ebben a kollekcióba lesznek az ApplicationUser-hez kapcsolt Assignment elemek
        public ICollection<Assignment> Assignments { get; set; }
    }
}
