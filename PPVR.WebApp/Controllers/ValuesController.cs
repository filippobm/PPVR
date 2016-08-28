using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace PPVR.WebApp.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public string Get()
        {
            return User.Identity.GetUserId();
        }
    }
}