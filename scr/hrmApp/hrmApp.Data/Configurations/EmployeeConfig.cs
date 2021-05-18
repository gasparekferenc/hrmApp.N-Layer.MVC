using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hrmApp.Core.Models;

namespace hrmApp.Data.Configurations
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(x => x.SurName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.ForeName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Birthplace)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.DateOfBirth)
                .IsRequired();

            builder.Property(x => x.PermPostCode)
                .HasMaxLength(4);

            builder.Property(x => x.PermCity)
                .HasMaxLength(50);

            builder.Property(x => x.PermAddress)
                .HasMaxLength(100);

            builder.Property(x => x.ResPostCode)
                .HasMaxLength(4);

            builder.Property(x => x.ResCity)
                .HasMaxLength(50);

            builder.Property(x => x.ResAddress)
                .HasMaxLength(100);

            builder.Property(x => x.EduLevel)
                .HasMaxLength(50);

            // kapcsolt adata
            builder.Property(x => x.EduDocId);

            builder.Property(x => x.EduDocDate);

            builder.Property(x => x.EduInstitute)
                .HasMaxLength(50);

            builder.Property(x => x.MothersName)
                .HasMaxLength(100);

            builder.Property(x => x.SSNumber)
                .HasMaxLength(9);

            builder.Property(x => x.TINumber)
                .HasMaxLength(10);

            builder.Property(x => x.BANumber)
                .HasMaxLength(26);

            builder.Property(x => x.OccValidDate);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(30);

            builder.Property(x => x.Email)
                .HasMaxLength(254);

            builder.Property(x => x.TravelAllowance);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.StartDate);

            builder.Property(x => x.EndDate);

            // a TAJ szám egyedi kell legyen!
            builder.HasIndex(x => x.SSNumber).IsUnique();

            //Ez a fontos!  Nem engedjük meg, az adatok kaszkádolt törlését!!!
            builder
                .HasOne(x => x.ProcessStatus)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.ProcessStatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(o => o.Job)
                .WithMany(po => po.Employees)
                .HasForeignKey(o => o.JobId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}