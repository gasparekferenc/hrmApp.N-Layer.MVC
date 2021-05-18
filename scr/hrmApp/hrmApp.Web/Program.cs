using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using hrmApp.Data;
using hrmApp.Data.Seed;
using hrmApp.Core.Models;
using hrmApp.Core.Constants;
using System.Security.Claims;

namespace hrmApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Read Configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            // Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            // létezik az adatbázis?
            using var scope = host.Services.CreateScope();
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                //context.Database.EnsureDeleted(); // forced delete database

                try
                {
                    Log.Information("Database access checking...");
                    if (context.Database.CanConnect())
                    {
                        Log.Information("Database access OK.");
                        Log.Information("Database context ensure checking...");
                        context.Database.EnsureCreated();
                        Log.Information("Database context ensure OK.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Fatal($"Can't access the database. Exit.\nError message:\n{ex.Message}");
                    // Ellenőrizni => OK! :-)
                    Environment.Exit(-1);
                }

                var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    Log.Information("Database Migrate...");
                    appContext.Database.Migrate();
                    Log.Information("Database Migrate OK!");
                }
                catch (Exception ex)
                {
                    Log.Information($"An error occurred while migrations. {ex}");
                    Environment.Exit(-2);
                    //throw;
                }


                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // Create default Roles 
                    CreateDefaultRoles(roleManager).Wait();

                    // Create default Admin
                    CreateDefaultAdmin(userManager, roleManager).Wait();
                }
                catch (Exception ex)
                {
                    Log.Fatal($"Can't create Default Admin. Exit.\nError message:\n{ex.Message}");
                    Environment.Exit(-3);
                    //throw;
                }


            }

            // Database seeding
            try
            {
                host.SeedDatabase();
                Log.Information("Seeding finish, or database  was seeded.");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "An error occurred seeding the DB");
            }

            // Run the hrmApp application
            try
            {
                Log.Information("Application Starting...");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() //Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        #region CreateDefaultAdmin()
        private static async Task CreateDefaultAdmin(UserManager<ApplicationUser> userManager,
                                                    RoleManager<IdentityRole> roleManager)
        {
            IConfiguration Configuration = Startup.StaticConfig;

            var defaultAdmin = new ApplicationUser();
            defaultAdmin = Configuration.GetSection("DefautAdmin").Get<ApplicationUser>();

            // var defaultAdmin = new ApplicationUser
            // {
            //     UserName = Configuration.GetSection("DefautAdmin").GetValue<string>("UserName"),    //"admin@email.com"
            //     SurName = Configuration.GetSection("DefautAdmin").GetValue<string>("SurName"),
            //     ForeName = Configuration.GetSection("DefautAdmin").GetValue<string>("ForeName"),
            //     PhoneNumber = Configuration.GetSection("DefautAdmin").GetValue<string>("PhoneNumber"),
            //     Description = Configuration.GetSection("DefautAdmin").GetValue<string>("Description"),
            //     Email = Configuration.GetSection("DefautAdmin").GetValue<string>("Email"),
            //     EmailConfirmed = true,
            //     PhoneNumberConfirmed = true
            // };
            defaultAdmin.EmailConfirmed = true;
            defaultAdmin.PhoneNumberConfirmed = true;
            var Password = Configuration.GetSection("DefautAdmin").GetValue<string>("Password");    //"Pa$$word123!"

            if (userManager.Users.All(u => u.Id != defaultAdmin.Id))
            {
                var userEmail = await userManager.FindByEmailAsync(defaultAdmin.Email);
                if (userEmail == null)
                {
                    await userManager.CreateAsync(defaultAdmin, Password); //Pa$$word123!

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
        #endregion

        #region CreateDefaultRoles(RoleManager<IdentityRole> roleManager)
        private static async Task CreateDefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in Roles.RoleNames)
            {
                await AddRoleAsync(roleName);
            }

            Log.Information("Default Roles created.");

            async Task AddRoleAsync(string RoleName)
            {
                if (!await roleManager.RoleExistsAsync(RoleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleName));
                }
            }
        }
        #endregion
    }


}
