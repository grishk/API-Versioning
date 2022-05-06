﻿using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Models;

namespace SeparateControllers.Extra.V2
{

    [ApiVersion("2.0")]
    [ODataRoutePrefix("People")]
    public class People2Controller : ODataController
    {
        
        public IHttpActionResult Get(int key)
        {
            return Ok(new Person(key){Desc = "V2"});
        }

        public IHttpActionResult Get(ODataQueryOptions<Person> options)
        {
            return Ok(new[] {new Person { Desc = "V2" } });
        }
    }
}