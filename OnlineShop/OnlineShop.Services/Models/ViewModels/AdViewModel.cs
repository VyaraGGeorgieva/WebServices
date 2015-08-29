﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OnlineShop.Models;

namespace OnlineShop.Services.Models.ViewModels
{
    public class AdViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public UserViewModel Owner { get; set; }

        public string Type { get; set; }

        public DateTime PostedOn { get; set; }

        public IEnumerable<CategoryViewModel> Category { get; set; }

        public static Expression<Func<Ad, AdViewModel>> Create
        {
            get
            {
                return ad => new AdViewModel()
                { 
                    Id = ad.Id,
                    Name = ad.Name,
                    Description = ad.Description,
                    Type = ad.Type.Name,
                    Category = ad.Categories
                        .Select(c => new CategoryViewModel()
                        {
                            Id = c.Id,
                            Name = c.Name
                        }),
                   
                    Owner = new UserViewModel()
                    {
                        Id = ad.Owner.Id,
                        Username = ad.Owner.UserName

                    },
                    PostedOn = DateTime.Now
                    
                    
                };
            }
        }
    }
}
