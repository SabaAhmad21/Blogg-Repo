using Blog_Website.Infrastructure.Interfaces;
using Blog_Website.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Website.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository) 
        { 
        
        _blogPostRepository = blogPostRepository;
        _blogPostLikeRepository = blogPostLikeRepository;

        }


        [HttpGet]
        public async Task<IActionResult> Index(string UrlHandle)
        {
          var blogPost = await _blogPostRepository.GetByUrlHandleAsync(UrlHandle);
            var blogDetailsVM = new BlogDetailsVM();

            if (blogPost != null)
            {
                var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);
           
                blogDetailsVM = new BlogDetailsVM
                {
                    Id = blogPost.Id,
                    PageTitle = blogPost.PageTitle,
                    PageContent = blogPost.PageContent,
                    Author = blogPost.Author,
                    PublishedDate = blogPost.PublishedDate,
                    Heading = blogPost.Heading,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,                  
                    TotalLikes = totalLikes,
                    Tags = blogPost.Tags
                 };



            }

            return View(blogDetailsVM);
        }
    }
}
