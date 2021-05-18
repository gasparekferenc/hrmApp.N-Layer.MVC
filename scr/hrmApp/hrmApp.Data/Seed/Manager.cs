using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using hrmApp.Core.Models;

namespace hrmApp.Data.Seed
{
    public static class Manager
    {

        public static IHost SeedDatabase(this IHost host)
        {
            var scope = host.Services.CreateScope();
            {

                try
                {
                    var services = scope.ServiceProvider;

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    //DefaultRoles.CreateAsync(roleManager).Wait();

                    //DefaultAdmin.CreateAsync(userManager, roleManager).Wait();

                    TestUsers.CreateAsync(userManager, roleManager).Wait();

                    TestData.Initialize(context).Wait();
                }
                catch (Exception e)
                {
                    Log.Information($"An error occurred while seeding default data. {e}");
                    //throw;
                }
            }
            return host;
        }
    }


}
