namespace Tourism.Core.Entities
{
    public class CommentImage : BaseEntity
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public string ImageUrl { get; set; }
    }
}