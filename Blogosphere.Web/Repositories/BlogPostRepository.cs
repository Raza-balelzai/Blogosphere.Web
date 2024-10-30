using Blogosphere.Web.Data;
using Blogosphere.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogosphere.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogosphereDbContext _context;
        public BlogPostRepository(BlogosphereDbContext context)
        {
            _context=context;
        }
        public async Task<BlogPost> AddBlogAsync(BlogPost blogPost)
        {
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteBlogAsync(Guid id)
        {
            var existingBlog= await _context.BlogPosts.FindAsync(id);
            if (existingBlog != null) 
            {
                _context.BlogPosts.Remove(existingBlog);
                await _context.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogsAsync()
        {
            return await _context.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogByIdAsync(Guid id)
        {
            return await _context.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<BlogPost?> GetBlogByUrlhandleAsync(string urlhandle)
        {
            return await _context.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.UrlHandle== urlhandle);
        }

        public async Task<BlogPost?> UpdateBlogAsync(BlogPost blogPost)
        {
          var existingBlog=  await _context.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Id==blogPost.Id);
            if (existingBlog != null)
            {
                existingBlog.Id= blogPost.Id;
                existingBlog.PageTitle=blogPost.PageTitle;
                existingBlog.Content=blogPost.Content;
                existingBlog.Author=blogPost.Author;
                existingBlog.PublishedDate=blogPost.PublishedDate;
                existingBlog.ShortDescription=blogPost.ShortDescription;
                existingBlog.UrlHandle=blogPost.UrlHandle;
                existingBlog.FeaturedImageUrl=blogPost.FeaturedImageUrl;
                existingBlog.Visible=blogPost.Visible;
                existingBlog.Heading=blogPost.Heading;
                existingBlog.Tags=blogPost.Tags;
                await _context.SaveChangesAsync();
                return existingBlog;

            }
            return null;
        }
    }
}
