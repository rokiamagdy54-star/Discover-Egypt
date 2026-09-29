using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class ReadyPlanConfiguration : IEntityTypeConfiguration<ReadyPlan>
    {
        public void Configure(EntityTypeBuilder<ReadyPlan> builder)
        {
            builder.ToTable("ReadyPlans");

            builder.Property(readyPlan => readyPlan.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(readyPlan => readyPlan.Company)
                   .WithMany(company => company.ReadyPlans)
                   .HasForeignKey(readyPlan => readyPlan.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(readyPlan => readyPlan.Guide)
                   .WithMany(guide => guide.ReadyPlans)
                   .HasForeignKey(readyPlan => readyPlan.GuideId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
