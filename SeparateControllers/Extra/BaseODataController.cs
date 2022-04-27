using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using SeparateControllers.Models;
using System.Web.Http;

namespace SeparateControllers.Extra

{
    public abstract class BaseODataController<T>: ODataController
        where T: EntityBase, new ()
    {
        public IHttpActionResult Get(int key)
        {
            return Ok(new T() { Id = key,  Name = $"Odata name of {typeof(T)}" });
        }

        public IHttpActionResult Get(ODataQueryOptions<T> options)
        {
            return Ok(new[] { new T { Id = 222, Name = $"Odata list nameame of {typeof(T)}" } });
        }

        public IHttpActionResult GetName([FromODataUri] int key)
        {
            return Ok(new T { Id = 222, Name = $"Odata list nameame of {typeof(T)}" }.Name) ;
        }
    }
}
