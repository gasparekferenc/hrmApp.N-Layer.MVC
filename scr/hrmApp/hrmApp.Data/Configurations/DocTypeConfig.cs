using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hrmApp.Core.Models;

namespace hrmApp.Data.Configurations
{
    public class DocTypeConfig : IEntityTypeConfiguration<DocType>
    {
        public void Configure(EntityTypeBuilder<DocType> builder)
        {
            builder.Property(x => x.TypeName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(x => x.Description)
                .HasMaxLength(1024);

            // builder.Property(x => x.MandatoryElement)
            //.HasDefaultValue(false);                

            builder.Property(x => x.IsActive)
                .IsRequired();
            //.HasDefaultValue(true);

            builder.Property(x => x.PreferOrder)
                .HasDefaultValue(100);
        }
    }
}