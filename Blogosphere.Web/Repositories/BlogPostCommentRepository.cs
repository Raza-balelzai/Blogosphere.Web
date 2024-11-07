using Blogosphere.Web.Data;
using Blogosphere.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogosphere.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BlogosphereDbContext blogosphereDbContext;

        public BlogPostCommentRepository(BlogosphereDbContext blogosphereDbContext)
        {
            this.blogosphereDbContext = blogosphereDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await blogosphereDbContext.BlogPostComment.AddAsync(blogPostComment);
            await blogosphereDbContext.SaveChangesAsync(); 
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogId(Guid blogPostId)
        {
           return await blogosphereDbContext.BlogPostComment.Where(x=>x.BlogPostId==blogPostId).ToListAsync();
        }
    }
}
