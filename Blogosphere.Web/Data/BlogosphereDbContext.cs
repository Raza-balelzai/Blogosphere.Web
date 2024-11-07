using Blogosphere.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogosphere.Web.Data
{
    public class BlogosphereDbContext:DbContext
    {
        public BlogosphereDbContext(DbContextOptions<BlogosphereDbContext> options) : base(options)
        {
            
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        public DbSet<BlogPostComment> BlogPostComment { get; set; }
    }
}
