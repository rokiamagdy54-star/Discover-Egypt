using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Place : BaseEntity
    {
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public string Description { get; set; }
        public string? DescriptionAr { get; set; }
        public string City { get; set; }
        public string? CityAr { get; set; }
        public Location Location { get; set; }
        public TimeSpan AverageVisitDuration { get; set; }
        public decimal TicketPrice { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<PlanPlace> PlanPlaces { get; set; } = new HashSet<PlanPlace>();
        public ICollection<PlaceReview> Reviews { get; set; } = new HashSet<PlaceReview>();
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>(); 
        public ICollection<PlacePhoto> Photos { get; set; } = new HashSet<PlacePhoto>();
    }
}
