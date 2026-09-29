using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }

        public ICollection<ReadyPlan> ReadyPlans { get; set; } = new HashSet<ReadyPlan>();
    }

}
