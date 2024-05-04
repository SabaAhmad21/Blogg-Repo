using Blog_Website.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Website.Infrastructure.Interfaces
{
    public interface IBlogPostLikeRepository
    {
         Task<int> GetTotalLikes(Guid BlogPostId);

        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
    }
}
