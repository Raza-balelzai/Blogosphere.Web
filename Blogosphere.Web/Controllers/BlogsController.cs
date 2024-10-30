using Blogosphere.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blogosphere.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        public BlogsController(IBlogPostRepository blog)
        {
            this.blogPostRepository = blog;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blog=await blogPostRepository.GetBlogByUrlhandleAsync(urlHandle);
            if (blog == null) {
                return NotFound("No Blog Found") ;
            
            } 

            return View(blog);
            
        }
    }
}
