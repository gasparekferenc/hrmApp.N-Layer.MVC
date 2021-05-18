using hrmApp.Core.Models.Base;

namespace hrmApp.Core.Models
{
    // Junction tábla a Project és az Intézmények között
    // Ez tartja nyilván, hogy mely Intézmény vesz részt az adott Projektben 
    // Many-to-Many kapcsolat

    public class ProjectOrganization : BaseEntity
    {
        //public int ProjectOrganizationID { get; set; }
        public int ProjectId { get; set; }
        public int OrganizationId { get; set; }

        public Project Project { get; set; }
        public Organization Organization { get; set; }
    }
}
