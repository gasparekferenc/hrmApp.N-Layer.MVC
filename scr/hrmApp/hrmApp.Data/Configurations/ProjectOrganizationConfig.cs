using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hrmApp.Core.Models;

namespace hrmApp.Data.Configurations
{
    public class ProjectOrganizationConfig : IEntityTypeConfiguration<ProjectOrganization>
    {
        public void Configure(EntityTypeBuilder<ProjectOrganization> builder)
        {
            builder.Property(x => x.ProjectId)
                .IsRequired();

            builder.Property(x => x.OrganizationId)
                .IsRequired();

            builder
                .HasOne(p => p.Project)
                .WithMany(po => po.ProjectOrganizations)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(o => o.Organization)
                .WithMany(po => po.ProjectOrganizations)
                .HasForeignKey(o => o.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}