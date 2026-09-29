using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Core.Entities
{
    public class PlanPlace : BaseEntity
    {
        public int ReadyPlanId { get; set; }
        public ReadyPlan ReadyPlan { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}
