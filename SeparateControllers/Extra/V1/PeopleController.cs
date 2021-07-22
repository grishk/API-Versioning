using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Models;

namespace SeparateControllers.Extra.V1
{
    
    [ODataRoutePrefix("People")]
    public class PeopleController : ODataController
    {
        public PeopleController()
        {
        }

        [ODataRoute("{id}")]
        public IHttpActionResult Get([FromODataUri] int id)
        {
            return Ok(new Person(id) { Desc = "V1" });
        }

        public IHttpActionResult Get()
        {
            return Ok(new[] {new Person { Desc = "V1" } });
        }
    }
}