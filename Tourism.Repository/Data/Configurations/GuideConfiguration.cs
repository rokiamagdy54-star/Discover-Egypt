using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class GuideConfiguration : IEntityTypeConfiguration<GuideProfile>
    {
        public void Configure(EntityTypeBuilder<GuideProfile> builder)
        {
            builder.ToTable("Guides");

            builder.HasKey(g => g.UserId);

            builder.Property(g => g.LicenseNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(g => g.LicenseNumber)
                   .IsUnique();

            builder.Property(g => g.LicenseImageUrl)
                   .HasMaxLength(500);

            builder.HasOne(g => g.User)
                   .WithOne(u => u.Guide)
                   .HasForeignKey<GuideProfile>(g => g.UserId);
        }
    }
}
