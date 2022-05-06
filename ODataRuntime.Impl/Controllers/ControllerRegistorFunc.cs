using ODataRuntime.Builders;
using System;
using System.Net;
using System.Net.Http;

namespace ODataRuntime.Impl.Controllers
{
    public static partial class ControllerRegistor
    {
        private static void RegisterFeeFunc(ControllerBuilder controllerBuilderFee) 
        {
            controllerBuilderFee.AddVersionNeutral();

            Func<int, decimal> getClientFee = (clientId) => 22222.3m;
            var actionBuilderGetFee = new ActionBuilderFromDelegate(controllerBuilderFee, "GetClientFee", getClientFee);
            actionBuilderGetFee
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(decimal))
                .AddODataRoute("GetClientFee(ClientId={clientId})")
                .AddSwaggerResponse(HttpStatusCode.OK, "Gets Client Fee by ClientId", typeof(decimal));
        }
    }
}

