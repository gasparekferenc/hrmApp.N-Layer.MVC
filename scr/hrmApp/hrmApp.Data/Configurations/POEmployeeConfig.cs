using hrmApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hrmApp.Data.Configurations
{
    public class POEmployeeConfig : IEntityTypeConfiguration<POEmployee>
    {
        public void Configure(EntityTypeBuilder<POEmployee> builder)
        {
            builder.Property(x => x.ProjectOrganizationId)
                .IsRequired();

            builder.Property(x => x.EmployeeId)
                .IsRequired();

            builder
                .HasOne(x => x.Employee)
                .WithMany(x => x.POEmployee)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(x => x.ProjectOrganization)
                .WithMany()
                .HasForeignKey(x => x.ProjectOrganizationId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        }
    }
}