using System.Web.Http;
using OnlineShop.Data;

namespace OnlineShop.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new OnlineShopContext())
        {
        }

        public BaseApiController(OnlineShopContext context)
        {
            this.Context = context;
        }

        protected OnlineShopContext Context { get; set; }
    };
}
