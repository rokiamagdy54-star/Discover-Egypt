using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class PlaceReviewConfiguration : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.Property(place => place.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(place => place.Description)
                   .HasMaxLength(1000);

            builder.Property(place => place.TicketPrice)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(place => place.Category)
                   .WithMany(category => category.Places)
                   .HasForeignKey(place => place.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(place => place.Tags)
                   .WithMany(tags => tags.Places)
                   .UsingEntity(join => join.ToTable("PlaceTags"));

            builder.OwnsOne(place => place.Location, location =>
            {
                location.Property(x => x.Latitude)
                        .HasColumnType("decimal(9,6)")
                        .IsRequired();

                location.Property(x => x.Longitude)
                        .HasColumnType("decimal(9,6)")
                        .IsRequired();
            });

            builder.Property(p => p.OpeningTime)
                   .IsRequired();

            builder.Property(p => p.ClosingTime)
                   .IsRequired();

            builder.Property(p => p.City)
                   .IsRequired()
                   .HasMaxLength(50);

        }
    }
}
