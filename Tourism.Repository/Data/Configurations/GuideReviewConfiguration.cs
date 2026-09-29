using Tourism.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tourism.Repository.Data.Configurations
{
    public class GuideReviewConfiguration : IEntityTypeConfiguration<GuideReview>
    {
        public void Configure(EntityTypeBuilder<GuideReview> builder)
        {
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Comment).IsRequired().HasMaxLength(1000);

            builder.HasOne(r => r.Tourist)
                   .WithMany(t => t.GuideReviews)
                   .HasForeignKey(r => r.TouristId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Guide)
                   .WithMany(g => g.GuideReviews)
                   .HasForeignKey(r => r.GuideId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Booking)
                   .WithMany()
                   .HasForeignKey(r => r.BookingId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}