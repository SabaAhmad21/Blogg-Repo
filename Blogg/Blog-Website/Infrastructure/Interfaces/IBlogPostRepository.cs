using Blog_Website.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Website.Infrastructure.Interfaces
{
    public interface IBlogPostRepository 
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetAsync(Guid id);

        Task<BlogPost> AddBlogAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateBlogAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteBlogAsync(Guid id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

    }
   
}
