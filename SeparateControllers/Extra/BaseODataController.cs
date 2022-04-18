using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SeparateControllers.Extra

{
    public abstract class BaseODataController<T>: ODataController
        where T: EntityBase, new ()
    {
        [ODataRoute("{id}")]
        public IHttpActionResult Get([FromODataUri] int id)
        {
            return Ok(new T() { Id = id,  Name = $"Name of {nameof(T)}" });
        }

        [MapToApiVersion("1.0"), ODataRoute("v1")]
        public IHttpActionResult GetV1()
        {
            return Ok(new[] { new T { Name = $"V1.0 Name of {nameof(T)}" } });
        }

        //[MapToApiVersion("2.0"), ODataRoute("v2")]
        //public IHttpActionResult GetV2()
        //{
        //    return Ok(new[] { new T { Name = $"V2.0 Name of {nameof(T)}" } });
        //}

        //[MapToApiVersion("3.0-alpha"), ODataRoute()]
        //public IList<T> GetV3a()
        //{
        //    return new[]
        //    {
        //        new T{Id = 1},
        //        new T{Id = 2},
        //        new T{Id = 3}
        //    };
        //}
    }

    //[ApiVersion("3.0")]
    //[ApiVersion("4.0")]
    //[ApiVersion("5.0-alpha")]
    //[ODataRoutePrefix("lm")]
    //public class LmController : BaseController<LocalMarket> 
    //{
    //}
}
