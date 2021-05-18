/*
Tartalmazza a munkavállalók (Eployee)
teljes ügymenetének folyamatát. Egyfajta log.
 */

using System;
using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class History : BaseEntity
    {
        public string Entry { get; set; }
        public int EntryType { get; set; }
        public DateTime EntryDate { get; set; }
        public bool AppUserEntry { get; set; } = false;
        // Opcionális, ha a bejegyzéshez tartozik csatolt dokumentum, annak Id-ja.
        public int? DocumentId { get; set; }
        public bool IsReminder { get; set; } = false;
        // Opcionális, ha be van állítva emlékeztető
        public DateTime? DeadlineDate { get; set; }

        // kapcsolt táblák

        // Kihez tartozik a bejegyzés
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}