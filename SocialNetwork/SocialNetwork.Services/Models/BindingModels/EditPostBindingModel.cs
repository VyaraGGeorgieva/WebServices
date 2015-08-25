using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Services.Models.BindingModels
{
    public class EditPostBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}
