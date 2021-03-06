using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace hrmApp.Web.Authorization
{
    public class AuthorizationNameHandler : AuthorizationHandler<AuthorizationNameRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationNameRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == requirement.AuthorizationName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
