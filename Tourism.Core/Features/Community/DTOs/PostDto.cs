namespace Tourism.Core.Features.Community.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? Title { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string? AuthorImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLikedByMe { get; set; }
        public List<string> Images { get; set; } = new();
    }
}