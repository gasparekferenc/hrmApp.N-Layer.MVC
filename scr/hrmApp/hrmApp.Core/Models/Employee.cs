using System;
using System.Collections.Generic;
using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class Employee : BaseEntityConcurrency
    {
        public string SurName { get; set; }
        public string ForeName { get; set; }
        public string FullName { get { return SurName + " " + ForeName; } }
        public string Birthplace { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PermPostCode { get; set; }
        public string PermCity { get; set; }
        public string PermAddress { get; set; }
        public string ResPostCode { get; set; }
        public string ResCity { get; set; }
        public string ResAddress { get; set; }
        public string EduLevel { get; set; }
        public string EduDocId { get; set; }
        public DateTime? EduDocDate { get; set; }
        public string EduInstitute { get; set; }
        public string MothersName { get; set; }
        public string SSNumber { get; set; }
        public string TINumber { get; set; }
        public string BANumber { get; set; }


        // Egyéb adatok
        public DateTime? OccValidDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool? TravelAllowance { get; set; } = false;
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        // a kapcsolt táblák kezelése

        // az adott munkavállalóhoz tartozó ProjectOrganization gyűjteménye
        public ICollection<POEmployee> POEmployee { get; set; }

        // A ProcessStatus tábla adatai
        // One-to-Many => Foregin Key: ProcessStatusId, Navigation property: ProcessStatus
        public int ProcessStatusId { get; set; }
        public ProcessStatus ProcessStatus { get; set; }

        // A Job tábla adatai
        // One-to-Many => ...
        public int JobId { get; set; }
        public Job Job { get; set; }

        // az adott munkavállalóhoz tartozó ELŐZMÉNY-ek gyűjteménye
        public ICollection<History> Histories { get; set; }

        // az adott munkavállalóhoz tartozó Documents-ek gyűjteménye
        public ICollection<Document> Documents { get; set; }



    }
}
