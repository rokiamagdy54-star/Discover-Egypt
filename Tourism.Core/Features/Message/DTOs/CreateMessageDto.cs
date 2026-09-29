using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Message.DTOs
{
    public class CreateMessageDto
    {
        [Required]
        public int ConversationId { get; set; }

        [Required, MaxLength(2000)]
        public string Content { get; set; }
    }
}