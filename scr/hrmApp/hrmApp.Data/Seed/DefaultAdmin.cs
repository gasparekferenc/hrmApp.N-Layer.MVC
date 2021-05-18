using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Serilog;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;


namespace hrmApp.Data.Seed
{
    public static class DefaultAdmin
    {

        public static async Task CreateAsync(UserManager<ApplicationUser> userManager,
                                            RoleManager<IdentityRole> roleManager)
        {
            //Get data from appsettings.json?
            DefaultAdminData defaultAdminData = new DefaultAdminData();

            string UserName = defaultAdminData.UserName;            
            string Email = defaultAdminData.Email;                 
            string Password = defaultAdminData.Password;           
            string SurName = defaultAdminData.SurName;             
            string ForeName = defaultAdminData.ForeName;           
            string PhoneNumber = defaultAdminData.PhoneNumber;     
            string Description = defaultAdminData.Description;     

            var defaultAdmin = new ApplicationUser
            {
                UserName = UserName,       
                SurName = SurName,
                ForeName = ForeName,
                PhoneNumber = PhoneNumber,
                Description = Description,
                Email = Email,              
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultAdmin.Id))
            {
                var userEmail = await userManager.FindByEmailAsync(Email);
                if (userEmail == null)
                {
                    await userManager.CreateAsync(defaultAdmin, Password);

                    var identityResult = await userManager.AddToRoleAsync(defaultAdmin, Roles.AdminRole);

                    IdentityRole identityRole = await roleManager.FindByNameAsync(Roles.AdminRole);

                    // To bind to Admin all the claims
                    foreach (var claimName in ClaimNames.ClaimName)
                    {
                        await roleManager.AddClaimAsync(identityRole, new Claim(claimName, ""));
                    }

                    Log.Information("Default Admin created.");
                }
            }
        }

    }
}
