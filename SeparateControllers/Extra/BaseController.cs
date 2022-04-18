using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SeparateControllers.Extra

{
    public abstract class BaseController<T>: ApiController
        where T: EntityBase, new ()
    {
        [Route("{id}")]
        public IHttpActionResult Get([FromODataUri] int id)
        {
            return Ok(new T() { Id = id,  Name = $"Name just of {nameof(T)}" });
        }

        [MapToApiVersion("3.0-alpha"), Route()]
        public IList<T> GetV3a()
        {
            return new[]
            {
                new T{Id = 10},
                new T{Id = 20},
                new T{Id = 30}
            };
        }
    }
}
