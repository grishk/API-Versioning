using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using SeparateControllers.Models;

namespace SeparateControllers.Extra.V2
{
   
    [ODataRoutePrefix("People")]
    public class PeopleController : ODataController
    {
        [ODataRoute("{id}")]
        
        public IHttpActionResult Get([FromODataUri] int id)
        {
            return Ok(new Person(id){Desc = "V2"});
        }

        public IHttpActionResult Get()
        {
            return Ok(new[] {new Person { Desc = "V2" } });
        }
    }
}