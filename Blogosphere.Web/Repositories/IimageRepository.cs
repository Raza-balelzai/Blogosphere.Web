namespace Blogosphere.Web.Repositories
{
    public interface IimageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
