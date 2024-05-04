using Blog_Website.Models.Domain;

namespace Blog_Website.Infrastructure.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddTagAsync(Tag tag);

        Task<Tag?> UpdateTagAsync(Tag tag);

        Task<Tag?> DeleteTagAsync(Guid id);

       
    }
}
