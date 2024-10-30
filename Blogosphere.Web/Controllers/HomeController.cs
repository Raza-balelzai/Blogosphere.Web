using Blogosphere.Web.Models;
using Blogosphere.Web.Models.DTOs;
using Blogosphere.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blogosphere.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger,IBlogPostRepository _blogpostRepository,ITagRepository tag)
        {
            _logger = logger;
            this.blogPostRepository = _blogpostRepository;
            this.tagRepository = tag;
        }

        public async Task<IActionResult> Index()
        {
            //Gettting all blogs
           var blogPosts= await blogPostRepository.GetAllBlogsAsync();

            //Getting All Tags
            var tags = await tagRepository.GetAllAsync();

            //pass both Models into HomeViewModel
            var HomeVM = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(HomeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
