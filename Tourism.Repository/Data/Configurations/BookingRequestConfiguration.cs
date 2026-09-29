using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class BookingRequestConfiguration : IEntityTypeConfiguration<Requset>
    {
        public void Configure(EntityTypeBuilder<Requset> builder)
        {
            builder.HasOne(br => br.Tourist)
               .WithMany()
               .HasForeignKey(br => br.TouristId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(br => br.Guide)
                   .WithMany()
                   .HasForeignKey(br => br.GuideId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
