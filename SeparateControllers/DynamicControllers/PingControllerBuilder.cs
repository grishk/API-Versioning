using Microsoft.AspNet.OData;
using SeparateControllers.DynamicBuilder;
using SeparateControllers.DynamicBuilder.Extensions;
using SeparateControllers.Models;
using System;
using System.Net;
using System.Net.Http;

namespace SeparateControllers.DynamicControllers
{
    public static class PingControllerBuilder
    {
        public static void Build() 
        {
            Func<int, int> ping = (ip) => 22222;
            var builder = AssemblyBuilder.CreateAssebly("Health");

            builder.BuildControllerBegin<Microsoft.Examples.PController>("Health")
                .BuildControllerEnd();

            builder.BuildControllerBegin<ODataController>("HealthDynamic")
                .AddVersionNeutral()
                .AddMethodBegin("Ping", typeof(int), typeof(int))
                .AddODataRoute("Ping(IP={ip})")
                .AddSwaggerResponse(HttpStatusCode.OK, "Ping OK")
                .AddSwaggerResponse(HttpStatusCode.BadRequest)
                .AddSwaggerResponse(HttpStatusCode.NotFound, "Nothing has been found")
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(int))
                .SetDelegate(ping)
                .AddMethodEnd()
                .BuildControllerEnd();
        }
    }
}
