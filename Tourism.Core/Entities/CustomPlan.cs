using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class CustomPlan : BasePlan
    {
            public string TouristId { get; set; }
            public TouristProfile Tourist { get; set; }
            public string? Notes { get; set; }
            public string? Destination { get; set; }
    }
}
