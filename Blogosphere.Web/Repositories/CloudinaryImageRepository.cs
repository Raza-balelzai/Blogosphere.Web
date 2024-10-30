
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

namespace Blogosphere.Web.Repositories
{
    public class CloudinaryImageRepository : IimageRepository
    {
        private readonly IConfiguration _configuration;
        private readonly Account account;
        public CloudinaryImageRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]);

        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var Client = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName,file.OpenReadStream()),
                DisplayName = file.FileName
            };
            var uploadResult =await Client.UploadAsync(uploadParams);
            if (uploadResult != null && uploadResult.StatusCode==HttpStatusCode.OK) 
            {
                return uploadResult.SecureUrl.ToString();
            }
            return null;
        }
    }
}
