using Blog_Website.Infrastructure.Interfaces;
using Blog_Website.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog_Website.Infrastructure.Implementations
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggDbContext _bloggDbContext;

        public BlogPostRepository(BloggDbContext bloggDbContext)
        {
            _bloggDbContext = bloggDbContext;
        }
        public async Task<BlogPost> AddBlogAsync(BlogPost blogPost)
        {
            await _bloggDbContext.AddAsync(blogPost);
            await _bloggDbContext.SaveChangesAsync();
            return blogPost;
        }
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _bloggDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }


        public async Task<BlogPost?> DeleteBlogAsync(Guid id)
        {
            var existingBlog = await _bloggDbContext.BlogPosts.FindAsync(id);
            if(existingBlog != null)
            {
                _bloggDbContext.BlogPosts.Remove(existingBlog);
                await _bloggDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }



        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _bloggDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateBlogAsync(BlogPost blogPost)
        {
            var existingBlog = await _bloggDbContext.BlogPosts.Include(x => x.Tags)
                  .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.PageContent = blogPost.PageContent;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await _bloggDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;

            //Task<IEnumerable<Tag>> IBlogPostRepository.GetAllAsync()
            //{
            //    throw new NotImplementedException();
            //}
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _bloggDbContext.BlogPosts.Include(x => x.Tags).
                FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }
    }
}
