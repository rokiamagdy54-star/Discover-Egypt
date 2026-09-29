namespace Tourism.Core.Entities
{
    public class PostImage : BaseEntity
    {
        public int PostId { get; set; }
        public CommunityPost Post { get; set; }
        public string ImageUrl { get; set; }
    }
}