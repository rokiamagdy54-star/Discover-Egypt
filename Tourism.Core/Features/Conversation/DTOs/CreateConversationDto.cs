using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Conversation.DTOs
{
    public class CreateConversationDto
    {
        [Required]
        public string GuideId { get; set; }
    }
}