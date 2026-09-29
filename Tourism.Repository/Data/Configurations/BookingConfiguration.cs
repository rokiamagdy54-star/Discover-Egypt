using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(booking => booking.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(booking => booking.Tourist)
                   .WithMany(tourist => tourist.Bookings)
                   .HasForeignKey(booking => booking.TouristId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(booking => booking.Guide)
                   .WithMany(guide => guide.Bookings)
                   .HasForeignKey(booking => booking.GuideId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(booking => booking.Plan)
                   .WithMany(basePlan => basePlan.Bookings)
                   .HasForeignKey(booking => booking.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
