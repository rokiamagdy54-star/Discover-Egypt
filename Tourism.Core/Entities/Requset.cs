using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism.Core.Enums;

namespace Tourism.Core.Entities
{
    public class Requset : BaseEntity
    {
        public string TouristId { get; set; }
        public ApplicationUser Tourist { get; set; }

        public string GuideId { get; set; }
        public ApplicationUser Guide { get; set; }
        public string Title { get; set; }
        public int CustomPlanId { get; set; }
        public CustomPlan CustomPlan { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
