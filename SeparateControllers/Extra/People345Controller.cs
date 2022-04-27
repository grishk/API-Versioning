using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Models;

namespace SeparateControllers.Extra
{
    [ApiVersion("3.0")]
    [ApiVersion("4.0")]
    [ApiVersion("5.0-alpha")]
    [ODataRoutePrefix("People")]
    public class People345Controller : ODataController
    {
        public IHttpActionResult Get(int key)
        {
            return Ok(new Person(key) { Desc = "V3,4" });
        }

        [MapToApiVersion("3.0"), ODataRoute()]
        public IHttpActionResult Get(ODataQueryOptions<Person> options)
        {
            return Ok(new[] { new Person { Desc = "V3" } });
        }

        [MapToApiVersion("4.0"), ODataRoute()]
        public IHttpActionResult Get(ODataQueryOptions<Person> options, string v4)
        {
            return Ok(new[] {new Person { Desc = "V4" } });
        }

        [MapToApiVersion("5.0-alpha"), ODataRoute()]
        public IList<Order> Get(ODataQueryOptions<Person> options, int? v5a)
        {
            return new[]
            {
                new Order{Id = 1},
                new Order{Id = 2},
                new Order{Id = 3}
            };
        }
    }
}