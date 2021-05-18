using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hrmApp.Core.Models;

namespace hrmApp.Data.Configurations
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // az alapokat tartalmazza az Identity?
            builder.Property(x => x.SurName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.ForeName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Description)
                .HasMaxLength(500);
        }
    }
}