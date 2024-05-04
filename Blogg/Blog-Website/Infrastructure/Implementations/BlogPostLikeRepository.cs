using Blog_Website.Infrastructure.Interfaces;
using Blog_Website.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Website.Infrastructure.Implementations
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggDbContext _bloggDbContext;

        public BlogPostLikeRepository(BloggDbContext bloggDbContext) 
        {
            _bloggDbContext = bloggDbContext;
        }

      

        public async Task<int> GetTotalLikes(Guid BlogPostId)
        {
           return  await _bloggDbContext.BlogPostLikes
                .CountAsync(x => x.BlogPostId==BlogPostId);

        }
        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _bloggDbContext.BlogPostLikes.AddAsync(blogPostLike);
            await _bloggDbContext.SaveChangesAsync();
            return blogPostLike;
        }
    }


}
