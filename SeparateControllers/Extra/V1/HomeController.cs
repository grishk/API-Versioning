using Asp.Versioning;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

namespace SeparateControllers.Extra.V1
{
	[ApiVersion("1.0")]
    [ODataRoutePrefix("Home")]
    //[Route("api/v{version:apiVersion}/home/{?id}")]
    public class HomeController : ODataController
    {
        public HomeController()
        {
        }

        public string Get(int id)
        {
            return "HomeV1";
        }

        
    }

  
}
