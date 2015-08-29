using Microsoft.AspNet.Identity.EntityFramework;
using OnlineShop.Data.Migrations;
using OnlineShop.Models;

namespace OnlineShop.Data
{
    using System.Data.Entity;

    public class OnlineShopContext : IdentityDbContext<ApplicationUser>
    {
        
        public OnlineShopContext()
            : base("name=OnlineShopContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OnlineShopContext, Configuration>());
        }

        public virtual IDbSet<Ad> Ads { get; set; }
        public virtual IDbSet<Category> Categories { get; set; }
        public virtual IDbSet<AdType> AdTypes { get; set; }
 
        public static OnlineShopContext Create()
        {
            return new OnlineShopContext();
        }
    }
}