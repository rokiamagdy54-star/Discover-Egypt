using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Conversation : BaseEntity
    {
        public string GuideId { get; set; }
        public GuideProfile Guide { get; set; }
        public string TouristId { get; set; }       
        public TouristProfile Tourist { get; set; }
        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
