using Asp.Versioning;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

namespace SeparateControllers.Extra.V2
{
	[ApiVersion("2.0")]
    [ODataRoutePrefix("Home")]
    //[Route("api/v{version:apiVersion}/home/{id}")]
    public class HomeController : ODataController
    {
        public string Get(int id)
        {
            return "HomeV2";
        }
    }
}
