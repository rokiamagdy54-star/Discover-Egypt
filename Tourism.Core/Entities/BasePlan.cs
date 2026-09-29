using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Enums;

namespace Tourism.Core.Entities
{
    public abstract class BasePlan : BaseEntity
    {
        public string Title { get; set; }
        public string? TitleAr { get; set; }
        public string Description { get; set; }
        public string? DescriptionAr { get; set; }
        public decimal Price { get; set; }
        public int? RequiredPoints { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public PlanStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
