namespace Tourism.Core.Features.Conversation.DTOs
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public string GuideId { get; set; }
        public string GuideName { get; set; }
        public string TouristId { get; set; }
        public string TouristName { get; set; }
        public int UnreadCount { get; set; }
        public MessagePreviewDto? LastMessage { get; set; }
    }

    public class MessagePreviewDto
    {
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}