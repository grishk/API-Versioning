using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;
using System.Net;
using System.Net.Http;

namespace ODataRuntime.Impl.ApiControllers
{
    public class ApiSite : Api<Site>
    {
        public override void Register(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.AddODataRoutePrefix(nameof(Site));
            controllerBuilder.AddVersion("0.4");
            var actionBuilderGet = new ActionBuilderFromBaseMethod(controllerBuilder, "Get", "DoGet");
            actionBuilderGet
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(Site))
                .AddSwaggerResponse(HttpStatusCode.OK, "Site by Id", typeof(Site));

            var actionBuilderPost = new ActionBuilderFromBaseMethod(controllerBuilder, "Post", "DoPost");
            actionBuilderPost
                .AddHttpVerb(HttpMethod.Post)
                .AddResponseType(typeof(Site))
                .AddSwaggerResponse(HttpStatusCode.OK, "Add Site", typeof(Site));
        }
    }
}
