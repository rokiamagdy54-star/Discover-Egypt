using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class PostImageConfiguration : IEntityTypeConfiguration<PostImage>
    {
        public void Configure(EntityTypeBuilder<PostImage> builder)
        {
            builder.Property(p => p.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(p => p.Post)
                   .WithMany(p => p.Images)
                   .HasForeignKey(p => p.PostId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}