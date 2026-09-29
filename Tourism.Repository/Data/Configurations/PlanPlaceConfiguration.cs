using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class PlanPlaceConfiguration : IEntityTypeConfiguration<PlanPlace>
    {
        public void Configure(EntityTypeBuilder<PlanPlace> builder)
        {

            builder.HasKey(pp => new { pp.ReadyPlanId, pp.PlaceId });

            builder.HasOne(pp => pp.ReadyPlan)
                .WithMany(rp => rp.PlanPlaces)
                .HasForeignKey(pp => pp.ReadyPlanId);

            builder.HasOne(pp => pp.Place)
                .WithMany(p => p.PlanPlaces)
                .HasForeignKey(pp => pp.PlaceId);
        }
    }
}
