using Blogosphere.Web.Models.Domain;
using Blogosphere.Web.Models.DTOs;
using Blogosphere.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogosphere.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;
        public AdminBlogPostsController(ITagRepository Tag, IBlogPostRepository _BlogPostRepository)
        {
            tagRepository = Tag;
            this.blogPostRepository = _BlogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //Get Tags From Repository
            var tags = await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //Map ViewModel To Domain Model
            var blogPostDomain = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                Content = addBlogPostRequest.Content,
                PageTitle = addBlogPostRequest.PageTitle,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                ShortDescription = addBlogPostRequest.ShortDescription,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };
            //Map Tags From selectedTags
            var SelectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var SeletedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetByIdAsync(SeletedTagIdAsGuid);
                if (existingTag != null)
                {
                    SelectedTags.Add(existingTag);
                }
            }
            //Mapping Tags Back To Domain Model 
            blogPostDomain.Tags = SelectedTags;
            await blogPostRepository.AddBlogAsync(blogPostDomain);
            return RedirectToAction("Add");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            //Call the Repository to fetch data
            var blogPosts = await blogPostRepository.GetAllBlogsAsync();


            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Retrieve the result from Repository
            var blogPost = await blogPostRepository.GetBlogByIdAsync(id);
            //fetch all the Tags for DropDown
            var allTags = await tagRepository.GetAllAsync();
            if (blogPost != null)
            {

                //Map the Domain model Into DTO
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = allTags.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray(),
                };


                //Pass Data To View
                return View(model);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //Map ViewModel tO Domain model
            var blogPostDomainModel = new BlogPost
            {
                 Id=editBlogPostRequest.Id,
                 Heading=editBlogPostRequest.Heading,
                 PageTitle=editBlogPostRequest.PageTitle,
                 Content=editBlogPostRequest.Content,
                 Author = editBlogPostRequest.Author,
                 ShortDescription=editBlogPostRequest.ShortDescription,
                 FeaturedImageUrl=editBlogPostRequest.FeaturedImageUrl,
                 UrlHandle=editBlogPostRequest.UrlHandle,
                 PublishedDate=editBlogPostRequest.PublishedDate,
                 Visible=editBlogPostRequest.Visible,
            };
            //Map Tags Into Domain Model 
            var selectedTags = new List<Tag>();
            foreach(var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetByIdAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags=selectedTags;
            //Submit Information to Repository
            var updatedBlog=await blogPostRepository.UpdateBlogAsync(blogPostDomainModel);
            if (updatedBlog != null)
            {
                //Redirect to Get
                //Show Success Notification
                return RedirectToAction("List");
            }
            //Show Error Notification
            return RedirectToAction("Edit");



        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Talk With Repository
            var deletedBlogPost = await blogPostRepository.DeleteBlogAsync(editBlogPostRequest.Id);
            if (deletedBlogPost != null)
            { 
                //Display the Response
                return RedirectToAction("List");
            }

            //Show Error message on Edit Page
            return RedirectToAction("Edit", new {id=editBlogPostRequest.Id});
        }
    }
}
