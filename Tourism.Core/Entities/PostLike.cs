namespace Tourism.Core.Entities
{
    public class PostLike : BaseEntity
    {
        public int PostId { get; set; }
        public CommunityPost Post { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}