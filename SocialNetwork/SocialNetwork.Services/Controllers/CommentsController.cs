using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;
using SocialNetwork.Services.Models.BindingModels;

namespace SocialNetwork.Services.Controllers
{
    //POST api/posts/{postId}/comment
    [Authorize]
    public class CommentsController : BaseController
    {
        [HttpPost]
        [Route("api/posts/{postId}/comments")]
        public IHttpActionResult AddCommentToPost (int postId,
            AddPostBindingModel model)
        {
            //check if the post exists
            var post = this.Context.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            //if the model is empty
            if (model==null)
            {
                return this.BadRequest("No values are sent!");
            }

            //check for validations
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            //if everything is OK
            var userId = this.User.Identity.GetUserId();
            var comment = new Comment()
            {
                Content = model.Content,
                PostedOn = DateTime.Now,
                AuthorId = userId
            };
            post.Comments.Add(comment);
            this.Context.SaveChanges();

            return this.Ok();
        }
    }
}
