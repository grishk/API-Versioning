using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Models;

namespace SeparateControllers.Extra
{
    [ApiVersion("3.0")]
    [ApiVersion("4.0")]
    [ApiVersion("5.0-alpha")]
    [ODataRoutePrefix("People")]
    public class PeopleController : ODataController
    {
        public PeopleController()
        {
        }

        [ODataRoute("{id}")]
        public IHttpActionResult Get([FromODataUri] int id)
        {
            return Ok(new Person(id) { Desc = "V3,4" });
        }

        [MapToApiVersion("3.0"), ODataRoute]
        public IHttpActionResult GetV3()
        {
            return Ok(new[] { new Person { Desc = "V3" } });
        }

        [MapToApiVersion("4.0"), ODataRoute]
        public IHttpActionResult GetV4()
        {
            return Ok(new[] {new Person { Desc = "V4" } });
        }
    }
}