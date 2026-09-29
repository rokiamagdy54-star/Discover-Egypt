using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class TouristConfiguration : IEntityTypeConfiguration<TouristProfile>
    {
        public void Configure(EntityTypeBuilder<TouristProfile> builder)
        {
            builder.ToTable("Tourists");

            builder.HasKey(t => t.UserId);

            builder.HasOne(t => t.User)
                   .WithOne(u => u.Tourist)
                   .HasForeignKey<TouristProfile>(t => t.UserId);
        }
    }
}
