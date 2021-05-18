/*
Az osztály a munkavállalók (Employee) csatolt dokumentumainak
adatait tárolja.
*/

using System;
using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class Document : BaseEntity
    {
        public string DocDisplayName { get; set; }
        public string DocPath { get; set; }
        public DateTime UploadedTimeStamp { get; set; }
        // [Display(Name = "Leírás")]
        public string Description { get; set; }


        // kapcsolat táblák
        // Melyik munkavállalóhoz tartozik a dokumentum
        public int EmployeeId { get; set; }
        // Mi a típusa a dokumentumnak
        public int DocTypeId { get; set; }

        // A kapcsolt táblák adatai        
        public Employee Employee { get; set; }
        public DocType DocType { get; set; }
    }
}