using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Core.Entities
{
    public class Message : BaseEntity
    {
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        public string SenderId { get; set; } 

        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}
