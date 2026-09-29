namespace Tourism.Core.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int PostId { get; set; }
        public CommunityPost Post { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; } = false;

        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new HashSet<Comment>();

        public ICollection<CommentImage> Images { get; set; } = new HashSet<CommentImage>();
        public ICollection<CommentLike> Likes { get; set; } = new HashSet<CommentLike>();
    }
}