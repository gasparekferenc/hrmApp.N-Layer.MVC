using hrmApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hrmApp.Data.Configurations
{
    public class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(x => x.ProjectName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.NumberOfEmployee)
                .IsRequired();

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1024);

            builder.Property(x => x.IsActive)
                .IsRequired();
            //.HasDefaultValue(true);
        }
    }
}

// hrmApp.Core