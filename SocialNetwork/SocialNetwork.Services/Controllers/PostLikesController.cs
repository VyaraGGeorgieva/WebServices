using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;

namespace SocialNetwork.Services.Controllers
{
    [Authorize]
    public class PostLikesController:BaseController
    {
        //POST api/postLikes/{postId}/likes
        [HttpPost]
        [Route("api/postLikes/{postId}/likes")]
        public IHttpActionResult Likes(int postId)
        {
            //check if the post exists
            var post = this.Context.Posts.Find(postId);
            if (post==null)
            {
                return this.NotFound();
            }

            
            var loggedInUser = this.User.Identity.GetUserId();
            if (post.AuthorId == loggedInUser)
            {
                return this.BadRequest("You cannot like your own post!");
            }

            var isAlreadyLiked = post.Likes
                .Any(pl => pl.UserId == loggedInUser);
            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this post");
            }

            post.Likes.Add(new PostLike()
            {
                UserId = loggedInUser
            });
            this.Context.SaveChanges();

            return this.Ok();
        }
    }
}
