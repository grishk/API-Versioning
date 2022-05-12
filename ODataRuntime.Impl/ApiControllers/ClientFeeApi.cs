using ODataRuntime.Builders;
using ODataRuntime.Interfaces;
using System;
using System.Net;
using System.Net.Http;

namespace ODataRuntime.Impl.ApiControllers
{
    public class ClientFeeApi : Api
    {
        private const string _ControllerName = "ClientFee";
        public ClientFeeApi() : base(_ControllerName)
        {
        }

        public override void Register(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.AddVersionNeutral();

            Func<int, decimal> getClientFee = (clientId) => 22222.3m;
            var actionBuilderGetFee = new ActionBuilderFromDelegate(controllerBuilder, "GetClientFee", getClientFee);
            actionBuilderGetFee
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(decimal))
                .AddODataRoute("GetClientFee(ClientId={clientId})")
                .AddSwaggerResponse(HttpStatusCode.OK, "Gets Client Fee by ClientId", typeof(decimal));
        }
    }
}
