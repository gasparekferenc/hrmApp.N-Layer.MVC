using System.Collections.Generic;

namespace hrmApp.Core.Constants
{
    public static class ClaimNames
    {
        public static List<string> ClaimName = new List<string>()
        {
            PolicyNames.BaseDataHandler,
            PolicyNames.AssignmentHandler,
            PolicyNames.ProjectOrganizationHendler,
            PolicyNames.UserHandler,
            PolicyNames.RoleHandler,
            PolicyNames.EmployeeHandler,
            PolicyNames.ReportHandler
        };
    }
}
