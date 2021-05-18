using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hrmApp.Core.Models;

namespace hrmApp.Data.Configurations
{
    public class AssignmentConfig : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.Property(x => x.ApplicationUserId)
                .IsRequired();

            builder.Property(x => x.OrganizationId)
                .IsRequired();

            builder
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.ApplicationUserId)
                // Ez nem szerepelt a korábbi ...designer állományban
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(x => x.Organization)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}