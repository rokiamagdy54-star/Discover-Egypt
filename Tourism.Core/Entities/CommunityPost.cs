namespace Tourism.Core.Entities
{
    public class CommunityPost : BaseEntity
    {
        public string Content { get; set; }
        public string? Title { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; } = false;

        public ICollection<PostImage> Images { get; set; } = new HashSet<PostImage>();
        public ICollection<PostLike> Likes { get; set; } = new HashSet<PostLike>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}