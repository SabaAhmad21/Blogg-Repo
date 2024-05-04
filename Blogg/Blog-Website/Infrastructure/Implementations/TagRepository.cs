using Blog_Website.Infrastructure.Interfaces;
using Blog_Website.Models.Domain;
using Blog_Website.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Blog_Website.Infrastructure.Implementations
{
    public class TagRepository : ITagRepository
    {

        private readonly BloggDbContext _bloggDbContext;
        public TagRepository(BloggDbContext bloggDbContext)
        {
            _bloggDbContext = bloggDbContext;
        }
        public async Task<Tag> AddTagAsync(Tag tag)
        {

            await _bloggDbContext.Tags.AddAsync(tag);
            await _bloggDbContext.SaveChangesAsync();

            return tag;
        }
        public async Task<Tag?> DeleteTagAsync(Guid id)
        {
         var editTag =   await _bloggDbContext.Tags.FindAsync(id);
            if( editTag!= null )
            {
                _bloggDbContext.Tags.Remove(editTag);
                await _bloggDbContext.SaveChangesAsync();
                return editTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
           return  await _bloggDbContext.Tags.ToListAsync();
        }

        public  Task<Tag?> GetAsync(Guid id)
        {
           return _bloggDbContext.Tags.FirstOrDefaultAsync(x=> x.Id==id);
        }

        public async Task<Tag?> UpdateTagAsync(Tag tag)
        {
            var editTag = await _bloggDbContext.Tags.FindAsync(tag.Id);

            if(editTag != null)
            {
                editTag.Name = tag.Name;
                editTag.DisplayName = tag.DisplayName;

                await _bloggDbContext.SaveChangesAsync();

                return editTag;
            }

            return null;
        }
     

    }
}
