using System.Globalization;

namespace Blogosphere.Web.Models.Domain
{
    public class BlogPostComment
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateAddded { get; set; }

    }
}
