using System.Web.Http;
using SocialNetwork.Data;

namespace SocialNetwork.Services.Controllers
{
    public class BaseController :ApiController
    {
        public BaseController()
            :this(new SocialNetworkContext())
        {            
        }

        public BaseController(SocialNetworkContext context)
        {
            this.Context = context;
        }
        protected SocialNetworkContext Context { get; set; }
    }
}
