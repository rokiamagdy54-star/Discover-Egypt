using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class ReadyPlan : BasePlan
    {
        public string GuideId { get; set; }
        public GuideProfile Guide { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<PlanPlace> PlanPlaces { get; set; } = new HashSet<PlanPlace>();
    }
}
