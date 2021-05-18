/*     Junction tábla a Project és az alkalmazottak között
    Ez tartja nyilván, hogy mely alkalmazottak, mely projektben vesznek részt
    many-to-Many kapcsolat */

using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    public class POEmployee : BaseEntity
    {
        public int ProjectOrganizationId { get; set; }
        public int EmployeeId { get; set; }

        public ProjectOrganization ProjectOrganization { get; set; }
        public Employee Employee { get; set; }
    }
}
