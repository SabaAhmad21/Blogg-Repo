namespace Blog_Website.Infrastructure.Interfaces
{
    public interface IImageRepository
    {
        Task <string> UploadAsync(IFormFile file);
    }
}
