
using Blogosphere.Web.Data;
using Blogosphere.Web.Models.Domain;
using Blogosphere.Web.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Blogosphere.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogosphereDbContext _context;
        public BlogPostLikeRepository(BlogosphereDbContext context)
        {
            _context = context;
            
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _context.BlogPostLike.AddAsync(blogPostLike);
            await _context.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid BlogPostId)
        {
           return await _context.BlogPostLike.Where(x=>x.BlogPostId == BlogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid BlogPostId)
        {
           return await _context.BlogPostLike.CountAsync(x=>x.BlogPostId == BlogPostId);

        }
    }
}
