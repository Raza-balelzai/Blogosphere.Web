using Azure;
using Blogosphere.Web.Data;
using Blogosphere.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Blogosphere.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogosphereDbContext _context;
        public TagRepository(BlogosphereDbContext context)
        {
            _context = context;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteTagAsync(Guid id)
        {
            var existingTag = await _context.Tags.FindAsync(id);
            if (existingTag != null)
            {
                _context.Tags.Remove(existingTag);
                await _context.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuerry,string? sortBy, string? sortDirection)
        {
            var querry= _context.Tags.AsQueryable();
            //filtering
            if (string.IsNullOrEmpty(searchQuerry) == false)
            {
                querry= querry.Where(x=>x.Name.Contains(searchQuerry) || x.DisplayName.Contains(searchQuerry)  );
            }
            //sorting
            if (string.IsNullOrEmpty(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    querry = isDesc ? querry.OrderByDescending(x => x.Name) : querry.OrderBy(x => x.Name);
                
                }
                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    querry = isDesc ? querry.OrderByDescending(x => x.DisplayName) : querry.OrderBy(x => x.DisplayName);

                }

            }
            //pagination
            return await querry.ToListAsync();
           //return await _context.Tags.ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(Guid id)
        {
            return await _context.Tags.FirstOrDefaultAsync(X => X.Id == id);
        }

        public async Task<Tag?> UpdateTagAsync(Tag tag)
        {
            var existingTag= await _context.Tags.FindAsync(tag.Id);
            if (existingTag != null)
            {

                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await _context.SaveChangesAsync();
                return existingTag;
              
            }
            return null;
        }
    }
}
