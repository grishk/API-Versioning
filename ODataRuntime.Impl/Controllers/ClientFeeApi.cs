using ODataRuntime.Builders;
using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNet.OData;

namespace ODataRuntime.Impl.Controllers
{
    public class ClientFeeApi : Api
    {
        public ClientFeeApi(AssemblyBuilder assemblyBuilder) : base(assemblyBuilder, "Fees", typeof(ODataController))
        {
        }

        protected override void Register(ControllerBuilder builder)
        {
            builder.AddVersionNeutral();

            Func<int, decimal> getClientFee = (clientId) => 22222.3m;
            var actionBuilderGetFee = new ActionBuilderFromDelegate(builder, "GetClientFee", getClientFee);
            actionBuilderGetFee
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(decimal))
                .AddODataRoute("GetClientFee(ClientId={clientId})")
                .AddSwaggerResponse(HttpStatusCode.OK, "Gets Client Fee by ClientId", typeof(decimal));
        }
    }
}

