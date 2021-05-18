using hrmApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace hrmApp.Data.Configurations
{
    public class PocessStatusConfig : IEntityTypeConfiguration<ProcessStatus>
    {
        public void Configure(EntityTypeBuilder<ProcessStatus> builder)
        {
            builder.Property(x => x.StatusName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.PreferOrder)
                .IsRequired()
                .HasDefaultValue(100);
        }
    }
}