using System.Web.Http;
using Microsoft.Web.Http;

namespace SeparateControllers.Extra.V2
{
    [ApiVersion("2.0")]
    [ControllerName("home")]
    //[Route("api/v{version:apiVersion}/home/{id}")]
    public class HomeController : ApiController
    {
        public string Get(int id)
        {
            return "HomeV2";
        }
    }
}
