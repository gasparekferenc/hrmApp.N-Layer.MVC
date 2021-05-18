using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hrmApp.Core.Models;

namespace hrmApp.Data.Configurations
{
    public class DocumentConfig : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.Property(x => x.DocDisplayName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.DocPath)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.UploadedTimeStamp)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1024);

            builder.Property(x => x.EmployeeId)
                .IsRequired();

            builder.Property(x => x.DocTypeId)
                .IsRequired();

            builder
                .HasOne(p => p.Employee)
                .WithMany(po => po.Documents)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(o => o.DocType)
                .WithMany(po => po.Documents)
                .HasForeignKey(o => o.DocTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        }
    }
}