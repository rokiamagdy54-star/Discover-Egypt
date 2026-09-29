using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasOne(f => f.User).WithMany()
               .HasForeignKey(f => f.UserId);

            builder.HasOne(f => f.Place).WithMany()
                .HasForeignKey(f => f.PlaceId);

            builder.HasIndex(f => new { f.UserId, f.PlaceId }).IsUnique();
        }
    }
}
