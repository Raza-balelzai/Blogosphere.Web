namespace Blogosphere.Web.Models.DTOs
{
    public class AddLikeRequest
    {
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
