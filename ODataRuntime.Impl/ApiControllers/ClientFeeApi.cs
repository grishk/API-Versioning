using ODataRuntime.Builders;
using ODataRuntime.Interfaces;
using System;
using System.Net;
using System.Net.Http;

namespace ODataRuntime.Impl.ApiControllers
{
    public class ClientFeeApi : UnboundApi {
        private const string _ApiName = "ClientFee";
        public ClientFeeApi() : base(_ApiName) { }

        public override void Register(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.AddVersionNeutral();

            Func<int, decimal> getClientFee = (clientId) => 22222.3m;
            var actionBuilderGetFee = new ActionBuilderFromDelegate(controllerBuilder, "GetClientFee", getClientFee);
            actionBuilderGetFee
                .AddHttpVerb(HttpMethod.Get)
                .SetResponseType(typeof(decimal))
                .SetODataRoute("GetClientFee(ClientId={clientId})")
                .AddSwaggerResponse(HttpStatusCode.OK, "Gets Client Fee by ClientId", typeof(decimal));
        }
    }
}
