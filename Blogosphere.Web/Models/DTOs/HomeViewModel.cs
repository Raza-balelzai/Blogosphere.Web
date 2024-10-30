using Blogosphere.Web.Models.Domain;

namespace Blogosphere.Web.Models.DTOs
{
    public class HomeViewModel
    {
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<BlogPost> BlogPosts { get; set; }
    }
}
