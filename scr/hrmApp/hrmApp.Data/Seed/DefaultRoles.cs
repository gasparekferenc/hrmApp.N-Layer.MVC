using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using hrmApp.Core.Constants;
using Serilog;

namespace hrmApp.Data.Seed
{
    public class DefaultRoles
    {
        public static async Task CreateAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in Roles.RoleNames)
            {
                await AddRoleAsync(roleName);
            }
            Log.Information("Default Roles added.");
            async Task AddRoleAsync(string RoleName)
            {
                if (!await roleManager.RoleExistsAsync(RoleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleName));
                }
            }
        }
    }
}

