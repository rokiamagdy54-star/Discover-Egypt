namespace Tourism.Core.Entities
{
    public class CommentLike : BaseEntity
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}