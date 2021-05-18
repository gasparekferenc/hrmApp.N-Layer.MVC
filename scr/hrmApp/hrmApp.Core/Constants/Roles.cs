using System.Collections.Generic;

namespace hrmApp.Core.Constants
{
    public static class Roles
    {
        public const string AdminRole = "Admin";
        public const string ManagerRole = "Manager";
        public const string OperatorRole = "Operator";
        public const string ReaderRole = "Reader";
        public static List<string> RoleNames = new List<string>()
        {
            AdminRole,
            ManagerRole,
            OperatorRole,
            ReaderRole
        };
    }
}