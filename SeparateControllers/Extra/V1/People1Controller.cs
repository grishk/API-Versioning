using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using SeparateControllers.Models;
using System.Web.Http;
using Asp.Versioning;

namespace SeparateControllers.Extra.V1
{

	[ApiVersion("1")]
    [ODataRoutePrefix("People")]
    public class People1Controller : ODataController
    {
        public People1Controller()
        {
        }

        public IHttpActionResult Get([FromODataUri] int key)
        {
            return Ok(new Person(key) { Desc = "V1" });
        }

        public IHttpActionResult Get(ODataQueryOptions<Person> options)
        {
            return Ok(new[] {new Person { Desc = "V1" } });
        }

        public IHttpActionResult GetDesc([FromODataUri] int key)
        {
            return Ok(new Person { Id = 222,  Desc= "V1 Odata list nameame of desc" }.Desc);
        }

    }
}