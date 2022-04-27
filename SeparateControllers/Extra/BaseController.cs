using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SeparateControllers.Extra

{
    public abstract class BaseController<T>: ApiController
        where T: EntityBase, new ()
    {
        public IHttpActionResult Get(int id)
        {
            return Ok(new T() { Id = id,  Name = $"Name of {nameof(T)}" });
        }

        public IHttpActionResult Get(Guid g)
        {
            return Ok(new[] { new T { Name = $"V1.0 Name of {nameof(T)}" } });
        }
    }
}
