using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public string TouristId { get; set; }
        public TouristProfile Tourist { get; set; }
    }
}
