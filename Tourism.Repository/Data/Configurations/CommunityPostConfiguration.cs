using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class CommunityPostConfiguration : IEntityTypeConfiguration<CommunityPost>
    {
        public void Configure(EntityTypeBuilder<CommunityPost> builder)
        {
            builder.Property(p => p.Content)
                   .IsRequired()
                   .HasMaxLength(5000);

            builder.Property(p => p.Title)
                   .HasMaxLength(200);

            builder.HasOne(p => p.Author)
                   .WithMany()
                   .HasForeignKey(p => p.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}