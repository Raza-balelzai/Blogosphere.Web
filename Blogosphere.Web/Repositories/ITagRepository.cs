using Blogosphere.Web.Models.Domain;

namespace Blogosphere.Web.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(string? searchQuerry=null,string? sortBy=null,string? sortDirection=null);
        Task<Tag?> GetByIdAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateTagAsync(Tag tag);
        Task<Tag?> DeleteTagAsync(Guid id);
    }
}
