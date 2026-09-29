using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Favorite : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
