using Blogosphere.Web.Models.Domain;
using Blogosphere.Web.Models.DTOs;

namespace Blogosphere.Web.Repositories
{
    public interface IBlogPostLikeRepository
    {
       Task<int> GetTotalLikes (Guid BlogPostId);
       Task<IEnumerable<BlogPostLike>> GetLikesForBlog (Guid BlogPostId);

       Task<BlogPostLike> AddLikeForBlog (BlogPostLike blogPostLike);
    }
}
