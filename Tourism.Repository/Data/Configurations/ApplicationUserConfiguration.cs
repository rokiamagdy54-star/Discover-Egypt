using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Gender)
                   .HasConversion<string>();

            builder.Property(u => u.BirthDate)
                   .HasColumnType("date");

            builder.HasOne(u => u.Nationality)
                   .WithMany(n => n.Users)
                   .HasForeignKey(u => u.NationalityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
