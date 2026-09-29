using Tourism.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tourism.Repository.Data.Configurations
{
    public class PlacePhotoConfiguration : IEntityTypeConfiguration<PlacePhoto>
    {
        public void Configure(EntityTypeBuilder<PlacePhoto> builder)
        {
            builder.Property(p => p.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(p => p.Place)
                   .WithMany(p => p.Photos)
                   .HasForeignKey(p => p.PlaceId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}