using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;

// Seed users for test purposes

namespace hrmApp.Data.Seed
{
    public static class TestUsers
    {
        public static async Task CreateAsync(UserManager<ApplicationUser> userManager,
                                                RoleManager<IdentityRole> roleManager)
        {
            // Manager
            var User = new ApplicationUser
            {
                UserName = "manager@email.com",
                Email = "manager@email.com",
                SurName = "Menedzser",
                ForeName = "User",
                PhoneNumber = "06 30 123 4567",
                Description = "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var Password = "Pa$$word123!";

            if (userManager.Users.All(u => u.Id != User.Id))
            {
                var userEmail = await userManager.FindByEmailAsync(User.Email);
                if (userEmail == null)
                {
                    await userManager.CreateAsync(User, Password);

                    await userManager.AddToRoleAsync(User, Roles.ManagerRole);

                    var identityRole = await roleManager.FindByNameAsync(Roles.ManagerRole);
                    await roleManager.AddClaimAsync(identityRole, new Claim(PolicyNames.BaseDataHandler, ""));
                    await roleManager.AddClaimAsync(identityRole, new Claim(PolicyNames.AssignmentHandler, ""));
                    await roleManager.AddClaimAsync(identityRole, new Claim(PolicyNames.ProjectOrganizationHendler, ""));
                    await roleManager.AddClaimAsync(identityRole, new Claim(PolicyNames.EmployeeHandler, ""));
                    await roleManager.AddClaimAsync(identityRole, new Claim(PolicyNames.ReportHandler, ""));

                    Log.Information($"{User.UserName} user created.");
                }
            }

            // Operator

            User = new ApplicationUser
            {
                UserName = "operator@email.com",
                SurName = "Operátor",
                ForeName = "User",
                PhoneNumber = "06 30 123 4567",
                Description = "Operátor...",
                Email = "operator@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            Password = "Pa$$word123!";

            if (userManager.Users.All(u => u.Id != User.Id))
            {
                var userEmail = await userManager.FindByEmailAsync(User.Email);
                if (userEmail == null)
                {
                    await userManager.CreateAsync(User, Password);

                    await userManager.AddToRoleAsync(User, Roles.OperatorRole);

                    var identityRole = await roleManager.FindByNameAsync(Roles.OperatorRole);
                    await roleManager.AddClaimAsync(identityRole, new Claim(PolicyNames.EmployeeHandler, ""));
                    await roleManager.AddClaimAsync(identityRole, new Claim(PolicyNames.ReportHandler, ""));

                    Log.Information($"{User.UserName} user created.");
                }
            }

            // Reader

            User = new ApplicationUser
            {
                UserName = "reader@email.com",
                SurName = "Rieder",
                ForeName = "User",
                PhoneNumber = "06 30 123 4567",
                Description = "Read Only user...",
                Email = "reader@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            Password = "Pa$$word123!";

            if (userManager.Users.All(u => u.Id != User.Id))
            {
                var userEmail = await userManager.FindByEmailAsync(User.Email);
                if (userEmail == null)
                {
                    await userManager.CreateAsync(User, Password);

                    await userManager.AddToRoleAsync(User, Roles.ReaderRole);

                    var identityRole = await roleManager.FindByNameAsync(Roles.ReaderRole);
                    await roleManager.AddClaimAsync(identityRole, new Claim(PolicyNames.ReportHandler, ""));

                    Log.Information($"{User.UserName} user created.");
                }
            }
        }
    }
}
