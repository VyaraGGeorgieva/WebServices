using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SocialNetwork.Data;
using SocialNetwork.Models;
using SocialNetwork.Services.Models.BindingModels;
using SocialNetwork.Services.Models.ViewModels;

namespace SocialNetwork.Services.Controllers
{
    [Authorize]
    public class PostsController:BaseController
    {
        //GET api/posts
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetPosts ()
        {
            var posts = this.Context.Posts
                .OrderBy(p=>p.PostedOn)
                .Select(p => p.Content);
            return this.Ok(posts);
        }

        //POST api/posts
        [HttpPost]
        public IHttpActionResult AddPost( [FromBody]AddPostBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (!this.ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            if (model == null)
            {
                return this.BadRequest("No values added!");
            }
            
            var wallOwner = this.Context.Users
                .FirstOrDefault(u => u.UserName == model.WallOwnerUserName);
            if (wallOwner == null)
            {
                return this.BadRequest(string.Format("User {0} does not exist", model.WallOwnerUserName));
            }

            var post = new Post()
            {
                AuthorId = loggedUserId,
                WallOwner = wallOwner,
                Content = model.Content,
                PostedOn = DateTime.Now
            };

            this.Context.Posts.Add(post);
            this.Context.SaveChanges();

            var data = this.Context.Posts
                .Where(p => p.Id == post.Id)
                .Select(PostViewModel.Create)
                .FirstOrDefault();
            return this.Ok(data);
        }
        

        //PUT api/posts/{id}
        [HttpPut]
        public IHttpActionResult EditPost(
            int id,
            [FromBody]EditPostBindingModel model)
        {
            //check if the post exists
            var post = this.Context.Posts.Find(id);

            if (post == null)
            {
                return this.NotFound();
            }

            var loggedInUser = this.User.Identity.GetUserId();
            if (loggedInUser != post.AuthorId)
            {
                return this.Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (model == null)
            {
                return this.BadRequest("No model data is sent!");
            }

            post.Content = model.Content;
            this.Context.SaveChanges();

            var data = this.Context.Posts
                .Where(p => p.Id == post.Id)
                .Select(PostViewModel.Create)
                .FirstOrDefault();
            return this.Ok(data);
        }

        //DELETE api/posts/{id}
        [Authorize (Roles = "Admin") ]
        public IHttpActionResult DeletePost(int id)
        {
            var post = this.Context.Posts.Find(id);
            if (post==null)
            {
                return this.NotFound();
            }

            this.Context.Posts.Remove(post);
            this.Context.SaveChanges();

            //need if he is the author of the post/ if he has posted
            var loggedInUser = this.User.Identity.GetUserId();
            if (!this.User.IsInRole("Admin")&& 
                loggedInUser!=post.AuthorId &&
                loggedInUser!=post.WallOwnerId)
            {
                return this.Unauthorized();
            }
            return this.Ok();
        }
    }
}
