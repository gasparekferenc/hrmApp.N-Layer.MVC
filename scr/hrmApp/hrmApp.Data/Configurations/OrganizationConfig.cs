using hrmApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hrmApp.Data.Configurations
{
    public class OrganizationConfig : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.Property(x => x.OrganizationName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.City)
              .HasMaxLength(50);

            builder.Property(x => x.Address)
                .HasMaxLength(100);

            builder.Property(x => x.ContactName)
                .HasMaxLength(30);

            builder.Property(x => x.ContactEmail)
                .HasMaxLength(254);

            builder.Property(x => x.ContactPhone)
                .HasMaxLength(30);

            builder.Property(x => x.Description)
                .HasMaxLength(1024);

            builder.Property(x => x.IsActive)
                .IsRequired();
            //.HasDefaultValue(true);

            builder.Property(x => x.PreferOrder)
                .HasDefaultValue(100);
        }
    }
}
