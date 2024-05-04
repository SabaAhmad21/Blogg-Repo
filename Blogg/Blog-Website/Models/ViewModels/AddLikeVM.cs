namespace Blog_Website.Models.ViewModels
{
    public class AddLikeVM
    {
        public Guid BlogPostId { get; set; }

        public Guid UserId { get; set; }
    }
}
