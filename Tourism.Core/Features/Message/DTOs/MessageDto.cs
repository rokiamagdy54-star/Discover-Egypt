namespace Tourism.Core.Features.Message.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}