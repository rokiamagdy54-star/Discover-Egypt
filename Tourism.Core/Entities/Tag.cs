using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Place> Places { get; set; } = new HashSet<Place>();
    }
}
