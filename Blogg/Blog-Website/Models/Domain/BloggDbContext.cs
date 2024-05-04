using Microsoft.EntityFrameworkCore;

namespace Blog_Website.Models.Domain
{
    public class BloggDbContext : DbContext
    {
        public BloggDbContext(DbContextOptions<BloggDbContext> options)
                    : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<BlogPostLike> BlogPostLikes { get; set; }

        public DbSet<BlogPostComment> BlogPostComments { get; set; }
    }
}