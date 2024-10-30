using Blogosphere.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blogosphere.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IimageRepository _imageRepository;
        public ImagesController(IimageRepository imageRepository)
        {
           _imageRepository = imageRepository;
            
        }
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var imageURL=await _imageRepository.UploadAsync(file);
            if (imageURL == null)
            {
                return Problem("Something went wrong while uploading Image!",null,(int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new { link=imageURL });
        }
    }
}
