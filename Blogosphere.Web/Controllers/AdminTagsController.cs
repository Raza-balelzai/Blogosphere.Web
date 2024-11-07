using Blogosphere.Web.Data;
using Blogosphere.Web.Models.Domain;
using Blogosphere.Web.Models.DTOs;
using Blogosphere.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blogosphere.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _TRepository;
        public AdminTagsController(ITagRepository TagRepo)
        {
            _TRepository = TagRepo;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest DTOtag)
        {
            ValidateAddTagRequest(DTOtag); 
            if (!ModelState.IsValid)
            {
                return View();
            }
            //Mapping AddTagRequest To the TagModel
            var tag = new Tag
            { 
                Name=DTOtag.Name,
                DisplayName=DTOtag.DisplayName
            };
            await _TRepository.AddAsync(tag);
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchQuerry,string? sortBy,string? sortDirection)
        {
            ViewBag.SearchQuerry = searchQuerry;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
           var tags= await _TRepository.GetAllAsync(searchQuerry,sortBy,sortDirection);
            return View(tags);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _TRepository.GetByIdAsync(id);
            if(tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest tag)
        {
            var ModelTag = new Tag
            {
                Id = tag.Id,
                Name = tag.Name,
                DisplayName = tag.DisplayName
            };
             var updatedTag= await _TRepository.UpdateTagAsync(ModelTag);
            if (updatedTag != null)
            {
                //Show Succes Notification
            }
            else
            {
            //Same page with failure Message

            }
            return RedirectToAction("Edit",new { id = tag.Id });
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest tag) 
        {
           var Deleted=await _TRepository.DeleteTagAsync(tag.Id);
            if (Deleted != null)
            {
                //Show Success message
                return RedirectToAction("List");

            }
            else
            {
            //Show an error notification

            }
            return RedirectToAction("Edit", new { id = tag.Id });
        }
        private void ValidateAddTagRequest(AddTagRequest tag)
        {
            if (tag.Name is not null && tag.DisplayName is not null)
            {
                if (tag.Name == tag.DisplayName) 
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be the same as Display-Name "); 

                }
            }
        }


    }
}
