using Blogosphere.Web.Models.Domain;
using Blogosphere.Web.Models.DTOs;
using Blogosphere.Web.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogosphere.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blog,
            IBlogPostLikeRepository BPLR,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blog;
            this.blogPostLikeRepository = BPLR;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blog = await blogPostRepository.GetBlogByUrlhandleAsync(urlHandle);

            if (blog == null)
            {
                return NotFound("No Blog Found");
            }
            //get Comments For Blog post
            var blogCommentsDomainModel= await blogPostCommentRepository.GetCommentsByBlogId(blog.Id);
            var blogCommentsForView=new List<BlogComment>();
            foreach (var blogComment in blogCommentsDomainModel) {
                blogCommentsForView.Add(new BlogComment
                {
                    Description = blogComment.Description,
                    DateAdded = blogComment.DateAddded,
                    UserName =(await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                });
            }

            var blogPostDetailsModel = new BlogDetailsViewModel
            {
                Id = blog.Id,
                Heading = blog.Heading,
                Content = blog.Content,
                ShortDescription = blog.ShortDescription,
                PageTitle = blog.PageTitle,
                FeaturedImageUrl = blog.FeaturedImageUrl,
                UrlHandle = blog.UrlHandle,
                Author = blog.Author,
                PublishedDate = blog.PublishedDate,
                Visible = blog.Visible,
                Tags = blog.Tags,
                TotalLikes = await blogPostLikeRepository.GetTotalLikes(blog.Id),
                Liked = false, // default to false for non-signed-in users
                Comments=blogCommentsForView,

            };

            if (signInManager.IsSignedIn(User))
            {
                var userId = userManager.GetUserId(User);
                if (userId != null)
                {
                    var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blog.Id);
                    blogPostDetailsModel.Liked = likesForBlog.Any(x => x.UserId == Guid.Parse(userId));
                }
            }

            return View(blogPostDetailsModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var DomainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAddded = DateTime.Now,

                };
                await blogPostCommentRepository.AddAsync(DomainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }
            return View();

        }
    }
}
