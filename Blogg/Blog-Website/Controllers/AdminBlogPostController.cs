using Blog_Website.Infrastructure.Interfaces;
using Blog_Website.Models.Domain;
using Blog_Website.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog_Website.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;

        }



        [HttpGet]
        public async Task<IActionResult> AddBlog()
        {
            //get tags from repository
            var tags = await _tagRepository.GetAllAsync();

            var model = new AddBlogPostVM
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> AddBlog(AddBlogPostVM addBlogPostVM)
        {
            //Mapping view model to Domain

            var blogPost = new BlogPost
            {
                Heading = addBlogPostVM.Heading,
                PageTitle = addBlogPostVM.PageTitle,
                PageContent = addBlogPostVM.PageContent,
                ShortDescription = addBlogPostVM.ShortDescription,
                FeaturedImageUrl = addBlogPostVM.FeaturedImageUrl,
                UrlHandle = addBlogPostVM.UrlHandle,
                PublishedDate = addBlogPostVM.PublishedDate,
                Author = addBlogPostVM.Author,
                Visible = addBlogPostVM.Visible,

            };
            // Map Tags from selected tags

            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostVM.SelectedTags)
            {

                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await _tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }

                //Mapping tags back to domain model
                blogPost.Tags = selectedTags;
            }




            await _blogPostRepository.AddBlogAsync(blogPost);

            return RedirectToAction("BlogList");
        }

        [HttpGet]
        public async Task<IActionResult> BlogList()
        {
            // call the repository
            var blogPosts = await _blogPostRepository.GetAllAsync();


            return View(blogPosts);
        }
        [HttpGet]
        public async Task<IActionResult> EditBlog(Guid id)
        {
            //Retrieve the result from the repository
            var blogPost = await _blogPostRepository.GetAsync(id);


            if (blogPost != null)
            {
                var tagsDomainModel = await _tagRepository.GetAllAsync();
                // map the domain model into the view model
                var model = new EditBlogPostVM
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    PageContent = blogPost.PageContent,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }

            // pass data to the view
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> EditBlog(EditBlogPostVM editBlogPostVM)
        {
            //map view model back to domain model
            var blogPostDomain = new BlogPost
            {
                Id = editBlogPostVM.Id,
                Heading = editBlogPostVM.Heading,
                PageTitle = editBlogPostVM.PageTitle,
                PageContent = editBlogPostVM.PageContent,
                ShortDescription = editBlogPostVM.ShortDescription,
                FeaturedImageUrl = editBlogPostVM.FeaturedImageUrl,
                UrlHandle = editBlogPostVM.UrlHandle,
                PublishedDate = editBlogPostVM.PublishedDate,
                Author = editBlogPostVM.Author,
                Visible = editBlogPostVM.Visible,

            };
            // map tags in Domain model

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostVM.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {

                    var foundTag = await _tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            
            blogPostDomain.Tags = selectedTags;
            //submit information repository to update
           var updatedBlog = await _blogPostRepository.UpdateBlogAsync(blogPostDomain);

            if (updatedBlog != null)
            {

                // show success notification
                return RedirectToAction("EditBlog");
            }


            // redirect to to Get

            return RedirectToAction("EditBlog");

        }
        

        public async Task<IActionResult> DeleteBlog(EditBlogPostVM editBlogPostVM)
        {
            //talk to repository
            var deletedBlogPost  =   await _blogPostRepository.DeleteBlogAsync(editBlogPostVM.Id);
            if (deletedBlogPost != null)
            {
                return RedirectToAction("BlogList");
            }
            return RedirectToAction("EditBlog", new {id=editBlogPostVM.Id});

        }   
    }
}
