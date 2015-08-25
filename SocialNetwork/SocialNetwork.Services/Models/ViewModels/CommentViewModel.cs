using System.Security.AccessControl;

namespace SocialNetwork.Services.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public UserViewModel Author { get; set; }
    }
}
