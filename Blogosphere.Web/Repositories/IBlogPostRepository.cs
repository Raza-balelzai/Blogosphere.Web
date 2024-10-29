using Blogosphere.Web.Models.Domain;

namespace Blogosphere.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllBlogsAsync();
        Task<BlogPost?> GetBlogByIdAsync(Guid id);
        Task<BlogPost> AddBlogAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateBlogAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteBlogAsync(Guid id);
    }
}
