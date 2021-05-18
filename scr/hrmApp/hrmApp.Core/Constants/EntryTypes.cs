using System.Collections.Generic;

namespace hrmApp.Core.Constants
{

    public static class EntryTypes
    {
        public const string Create = "Létrehozás";
        public const string Update = "Adatmódosítás";
        public const string Delete = "Törlés";
        public const string ReEnrollment = "Újra felvétel";
        public const string StatusChange = "Státusz változás";
        public const string OrganizationChange = "Szervezet váltás";
        public const string UploadDocument = "Dokumentum feltöltés";
        public const string DownloadDocument = "Dokumentum letöltés";
        public const string DeleteDocument = "Dokumentum törlés";
        public const string Memo = "Feljegyzés...";
        public const string MemoWithReminder = "Emlékeztető =>";
        public static List<string> Names = new List<string>()
        {
            Create,
            Update,
            Delete,
            ReEnrollment,
            StatusChange,
            OrganizationChange,
            UploadDocument,
            DownloadDocument,
            DeleteDocument,
            Memo,
            MemoWithReminder
        };

    }
}