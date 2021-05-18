/*
A DocType tartalmazza felhasználóhoz csatolt dokumentumok besorolását
- közvetítőlap
- orvosi alkalmassági igazolás
- bankszámla nyilatkozat
stb.
*/

using System.Collections.Generic;
using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class DocType : BaseEntityConcurrency
    {
        public string TypeName { get; set; }
        public string Description { get; set; }
        public bool MandatoryElement { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int PreferOrder { get; set; } = 100;

        public ICollection<Document> Documents { get; set; }

    }
}