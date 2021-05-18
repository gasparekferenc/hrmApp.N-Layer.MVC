using hrmApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hrmApp.Data.Configurations
{
    public class HistoryConfig : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
        {
            builder.Property(x => x.Entry)
                .IsRequired()
                .HasMaxLength(1024);

            builder.Property(x => x.EntryDate)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.AppUserEntry)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DocumentId);

            builder.Property(x => x.IsReminder)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DeadlineDate);

            builder
                .HasOne(x => x.ApplicationUser)
                .WithMany()
                .HasForeignKey(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(x => x.Employee)
                .WithMany(x => x.Histories)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();


        }
    }
}