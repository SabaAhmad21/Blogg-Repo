using Blog_Website.Infrastructure.Interfaces;
using Blog_Website.Models.Domain;
using Blog_Website.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Website.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
      
        private readonly ITagRepository _tagRepository;

        public AdminTagsController(ITagRepository tagRepository) 
        {
        _tagRepository = tagRepository;
        
        }

        
        [HttpGet]
        public IActionResult AddTag()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AddTag")]
        public async Task<IActionResult> AddTag(AddTagVM tagModel) 
        {
            //Mapping addTag to Tag Domain model
            var tag = new Tag
            {
                Name = tagModel.Name,
                DisplayName = tagModel.DisplayName
            };

            await _tagRepository.AddTagAsync(tag);

            return RedirectToAction("List");

        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // use DbContext to read the tags
            var tags =await _tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> EditTag(Guid id)
        {
            //1st method
            //var editTag = _bloggDbContext.Tags.Find(tag.Id);
            //2nd method
            var tag = await _tagRepository.GetAsync(id);
            if (tag != null)
            {
                var editTag  = new EditTagVM 
                { 
                    Id= tag.Id,
                    Name = tag.Name, 
                    DisplayName = tag.DisplayName 
                };
                return View(editTag);

            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> EditTag(EditTagVM editTagVM)
        {
            var tag = new Tag
            {
                Id = editTagVM.Id,
                Name = editTagVM.Name,
                DisplayName = editTagVM.DisplayName

            };
           
           var updatedTag = await _tagRepository.UpdateTagAsync(tag);
            if(updatedTag != null)
            {
                //Show success notification
                return RedirectToAction("List");
            }
            else
            {
                //Show error
            }



            return RedirectToAction("List", new { id = editTagVM.Id });
            //_bloggDbContext.Tags.Update(tag);
            //_bloggDbContext.SaveChanges();
            //return RedirectToAction("List");

        }

        [HttpPost]
        public async Task<IActionResult> DeleteTag(EditTagVM editTagVM) 
        {
          var deletedTag = await  _tagRepository.DeleteTagAsync(editTagVM.Id);

            if(deletedTag != null)
            {
                return RedirectToAction("List");
            }
         
            //Show error message
            return RedirectToAction("List",new {id = editTagVM.Id});
        }
    }
}
