﻿using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;
using System.Net;
using System.Net.Http;

namespace ODataRuntime.Impl.ApiControllers
{
    public class ApiClient : Api<Client>
    {
        public override void Register(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.AddODataRoutePrefix(nameof(Client));
            controllerBuilder.AddVersion("0.3");
            var actionBuilderGet = new ActionBuilderFromBaseMethod(controllerBuilder, "Get", "DoGet");
            actionBuilderGet
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(Client))
                .AddSwaggerResponse(HttpStatusCode.OK, "Client by Id", typeof(Client));
        }
    }
}