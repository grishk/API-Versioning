using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using System.Net;
using System.Net.Http;
using ODataRuntime.Controllers;

namespace ODataRuntime.Impl.Controllers
{
    public class SiteApi : Api<Site>
    {
        public SiteApi(AssemblyBuilder assemblyBuilder) : base(assemblyBuilder)
        {
        }

        protected override void Register(ControllerBuilder builder)
        {
            builder.AddODataRoutePrefix(nameof(Site));
            builder.AddVersion("0.4");
            var actionBuilderGet = new ActionBuilderFromBaseMethod(builder, "Get", "DoGet");
            actionBuilderGet
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(Site))
                .AddSwaggerResponse(HttpStatusCode.OK, "Site by Id", typeof(Site));

            var actionBuilderPost = new ActionBuilderFromBaseMethod(builder, "Post", "DoPost");
            actionBuilderPost
                .AddHttpVerb(HttpMethod.Post)
                .AddResponseType(typeof(Site))
                .AddSwaggerResponse(HttpStatusCode.OK, "Add Site", typeof(Site));
        }
    }
}
