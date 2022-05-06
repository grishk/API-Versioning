using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using System.Net;
using System.Net.Http;

namespace ODataRuntime.Impl.Controllers
{
    public static partial class ControllerRegistor
    {
        private static void RegisterSite(ControllerBuilder controllerBuilderSite)
        {
            controllerBuilderSite.AddODataRoutePrefix(nameof(Site));
            controllerBuilderSite.AddVersion("0.4");
            var actionBuilderGet = new ActionBuilderFromBaseMethod(controllerBuilderSite, "Get", "DoGet");
            actionBuilderGet
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(Site))
                .AddSwaggerResponse(HttpStatusCode.OK, "Site by Id", typeof(Site));

            var actionBuilderPost = new ActionBuilderFromBaseMethod(controllerBuilderSite, "Post", "DoPost");
            actionBuilderPost
                .AddHttpVerb(HttpMethod.Post)
                .AddResponseType(typeof(Site))
                .AddSwaggerResponse(HttpStatusCode.OK, "Add Site", typeof(Site));
        }
    }
}
