using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class BasePlanConfiguration : IEntityTypeConfiguration<BasePlan>
    {
        public void Configure(EntityTypeBuilder<BasePlan> builder)
        {
            builder.UseTpcMappingStrategy();

            builder.Property(p => p.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.StartDateTime)
                   .IsRequired();

            builder.Property(p => p.EndDateTime)
                   .IsRequired();

            builder.Property(p => p.Status)
                   .IsRequired();

            builder.Property(p => p.ImageUrl)
                   .HasMaxLength(500);
        }
    }
}
