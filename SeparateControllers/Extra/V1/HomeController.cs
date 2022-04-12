using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Models;

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
