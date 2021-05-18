using System.Reflection;
using hrmApp.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hrmApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Speed up possibility, cause the EF does not inspect the change of data in memory:
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<ProjectOrganization> ProjectOrganizations { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<POEmployee> POEmployees { get; set; }

        public DbSet<ProcessStatus> ProcessStatuses { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public DbSet<History> Histories { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocType> DocTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Gathering the info for config to Database stuff  => See the above in Configuration namespace...
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}