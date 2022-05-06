using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using System.Net;
using System.Net.Http;

namespace ODataRuntime.Impl.Controllers
{
    public static partial class ApiBuilder
    {
        private static void RegisterClient(ControllerBuilder controllerBuilderClient)
        {
            controllerBuilderClient.AddODataRoutePrefix(nameof(Client));
            controllerBuilderClient.AddVersion("0.3");
            var actionBuilderGet = new ActionBuilderFromBaseMethod(controllerBuilderClient, "Get", "DoGet");
            actionBuilderGet
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(Client))
                .AddSwaggerResponse(HttpStatusCode.OK, "Client by Id", typeof(Client));
        }
    }
}
