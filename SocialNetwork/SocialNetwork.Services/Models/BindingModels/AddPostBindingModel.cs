using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Services.Models.BindingModels
{
    public class AddPostBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }

        [Required]
        public string WallOwnerUserName { get; set; }
    }
}
