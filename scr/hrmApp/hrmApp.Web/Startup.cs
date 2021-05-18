using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper;
using FluentValidation.AspNetCore;
using hrmApp.Data;
using hrmApp.Core.Constants;
using hrmApp.Web.Constants;
using hrmApp.Web.Authorization;
using hrmApp.Core.Repositories;
using hrmApp.Data.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Data.UnitOfWorks;
using hrmApp.Core.Services;
using hrmApp.Services.Services;
using hrmApp.Web.Validators;
using hrmApp.Core.Models;

using hrmApp.Web.Extensions;
using Serilog;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;

namespace hrmApp
{
    public class Startup
    {


        #region Startup (IConfiguration configuration)
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }

        public static IConfiguration StaticConfig { get; private set; }

        public IConfiguration Configuration { get; }
        #endregion        

        #region ConfigureServices
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // SQL Server database connection
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    // .EnableSensitiveDataLogging()    // Részleteshibakereséshez
                    );

            // MailSettings from appsettings.json
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                options.SignIn.RequireConfirmedAccount = false)         // Ezt élesben true-ra kell állítani
                .AddEntityFrameworkStores<ApplicationDbContext>()
                //.AddDefaultUI()
                .AddDefaultTokenProviders();

            // Must be called after calling AddIdentity or AddDefaultIdentity.
            services.Configure<IdentityOptions>(options =>
                {
                    // Default Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                    // Default Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = BaseValues.PaswordMinLength;
                    options.Password.RequiredUniqueChars = 1;
                    // Default SignIn settings.
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    // Default User settings.
                    options.User.AllowedUserNameCharacters =
                                                            "abcdefghijklmnopqrstuvwxyz" +
                                                            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                                            "0123456789" +
                                                            "-._@+";
                    options.User.RequireUniqueEmail = true;
                });

            // Must be called after calling AddIdentity or AddDefaultIdentity.
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "hrmApp";
                options.Cookie.HttpOnly = true;
                // Confirmation e-mail timeot
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                // ReturnUrlParameter requires => using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;

            });
            // End of Identity settings


            // AddAuthorization setting 
            services.AddAuthorization(options =>
            {
                foreach (var policyName in ClaimNames.ClaimName)
                {
                    options.AddPolicy(policyName, policy =>
                    policy.Requirements.Add(new AuthorizationNameRequirement(policyName)));
                }

                // The fallback authentication policy requires all users to be authenticated
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                        .RequireAuthenticatedUser()
                                        .Build();
            });

            // Authorization (DI)
            services.AddSingleton<IAuthorizationHandler, AuthorizationNameHandler>();

            // AddAuthorization setting end

            // Mail Service
            services.AddTransient<IMailService, MailService>();

            // File Service
            services.AddTransient<IFileService, FileService>();


            // Dependecy Injection registrations
            ServiceRegistrationDI(services);

            services.AddControllersWithViews()
                .AddSessionStateTempDataProvider();
            services.AddRazorPages()
                .AddSessionStateTempDataProvider();

            //Session kezelés beállítása
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = ".hrmApp.Session";
                options.Cookie.HttpOnly = true;     // Client-side scripting disabled 
                options.Cookie.IsEssential = true;  // If true then consent policy checks may be bypassed.
            });



            // AutoMapper DI
            services.AddAutoMapper(typeof(Startup));

            // FuentValidarors registrations in MVC
            ServiceRegistrationFluentValidators(services);

        }
        #endregion

        #region Configure
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                // for internal test only
                //app.UseExceptionHandler("/Error");
                //app.UseStatusCodePagesWithReExecute("/Error/NotFound/{0}");
                //app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/NotFound/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseStaticFiles();
            // ???
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Project}/{action=Index}/{id?}");
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
            });
        }
        #endregion

        #region ServiceRegistrationDI(IServiceCollection services)
        private void ServiceRegistrationDI(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IDocTypeRepository, DocTypeRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPOEmployeeRepository, POEmployeeRepository>();
            services.AddScoped<IProcessStatusRepository, ProcessStatusRepository>();
            services.AddScoped<ProjectOrganizationRepository, ProjectOrganizationRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IDocTypeService, DocTypeService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IPOEmployeeService, POEmployeeService>();
            services.AddScoped<IProcessStatusService, ProcessStatusService>();
            services.AddScoped<IProjectOrganizationService, ProjectOrganizationService>();
            services.AddScoped<IProjectService, ProjectService>();
        }
        #endregion

        #region ServiceRegistrationFluentValidators(IServiceCollection services)
        // FluentValidarors registrations
        private void ServiceRegistrationFluentValidators(IServiceCollection services)
        {
            services.AddMvc()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<ApplicationUserDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<AssignmentDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<DocTypeDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<DocumentDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<EmployeeDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<HistoryDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<JobDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<OrganizationDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<POEmployeeDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<ProcessStatusDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<ProjectDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<ProjectOrganizationDTOValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<ConfirmEmailViewModelValidator>();

                    fv.RegisterValidatorsFromAssemblyContaining<NewEmployeeViewModelValidator>();
                });
        }
        #endregion
    }
}
