using Microsoft.AspNetCore.Authorization;

namespace hrmApp.Web.Authorization
{
    public class AuthorizationNameRequirement : IAuthorizationRequirement
    {
        public string AuthorizationName { get; private set; }
        public AuthorizationNameRequirement(string authorizationName)
        {
            AuthorizationName = authorizationName;
        }
    }
}
