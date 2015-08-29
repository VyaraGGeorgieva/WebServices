using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using OnlineShop.Models;
using OnlineShop.Services.Models.BindingModels;
using OnlineShop.Services.Models.ViewModels;

namespace OnlineShop.Services.Controllers
{
    [Authorize]
    public class AdsController:BaseApiController
    {
        //GET api/ads
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetAds()
        {
            
            var ads = this.Context.Ads
                .Where(a => a.Status == AdStatus.Open)
                .OrderBy(a => a.Type.Name == "Diamond")
                .ThenByDescending(a => a.Type.Name == "Premium")
                .ThenByDescending(a => a.Type.Name == "Normal")
                .ThenByDescending(a => a.PostedOn)
                .Select(AdViewModel.Create);    
            return this.Ok(ads);
        }

        [HttpPost]
        public IHttpActionResult CreateAd([FromBody]CreateAdBindingModel model)
        {
            //check if the user is logged-in
            var loggedUser = this.User.Identity.GetUserId();
            var user = this.Context.Users.FirstOrDefault(u => u.Id == loggedUser);
            
            //check if the model is not null

            if (model==null)
            {
                return this.BadRequest("No vlues added!");
            }
            //check if the state of the model is valid
            if (!this.ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            //validate that thee is at least one Category and mnot more than 3
            if (!model.Categories.Any() || model.Categories.Count() > 3)
            {
                return this.BadRequest("Categories count cannot be " + model.Categories.Count());
            }

            //check if the type ID is real
            var typeId = this.Context.AdTypes
                .FirstOrDefault(c => c.Id == model.TypeId);
            if (typeId == null)
            {
                return this.BadRequest("No type with id " + model.TypeId + " found!");
            }

            //check if the category ID is real

            var categories = new List<Category>();
            foreach (var categoryId in model.Categories)
            {
                var dbCategory = this.Context.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (dbCategory == null)
                {
                    return this.BadRequest("No category with id " + model.TypeId + " found!");
                }

                categories.Add(dbCategory);
            }

            var ad = new Ad()
            {
                Categories = categories,
                Name = model.Name,
                Description = model.Description,
                Type = typeId,
                Price = model.Price,
                Owner = user
           };
            
            this.Context.Ads.Add(ad);
            this.Context.SaveChanges();

            var result = Context.Ads
                .Where(a => a.Id == ad.Id)
                .Select(AdViewModel.Create)
                .FirstOrDefault();

            return this.Ok(result);
        }

        [Authorize]
        [Route("api/ads/{id}/close")]
        public IHttpActionResult PutCloseAd(int id)
        {
            var ad = this.Context.Ads.FirstOrDefault(a => a.Id == id);
            if (ad == null)
            {
                return this.BadRequest("No ad with id " + id);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            if (ad.OwnerId != loggedUserId)
            {
                return this.BadRequest("You are not the owner of the ad");
            }

            ad.ClosedOn = DateTime.Now;
            this.Context.SaveChanges();
            return this.Ok();

    }
}
