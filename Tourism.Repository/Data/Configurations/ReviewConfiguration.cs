using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<PlaceReview>
    {
        public void Configure(EntityTypeBuilder<PlaceReview> builder)
        {
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Comment).IsRequired().HasMaxLength(1000);

            builder.HasOne(r => r.Tourist)
                   .WithMany(t => t.PlaceReviews)
                   .HasForeignKey(r => r.TouristId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Place)
                   .WithMany(p => p.Reviews)
                   .HasForeignKey(r => r.PlaceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
