using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;
using ODataRuntime.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ODataRuntime.Impl.ApiControllers
{
    public class ApiSite : ModelApi<Site> {
        public override void Register(ControllerBuilder controllerBuilder) {
            controllerBuilder.SetRoute(nameof(Site));
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

            var actionBuilderCount = new ActionBuilderFromDelegate(controllerBuilder, "Counter", Count);
            actionBuilderCount
                .AddHttpVerb(HttpMethod.Get)
                .AddODataRoute("({key})/Counter")
                .AddResponseType(typeof(double))
                .AddSwaggerResponse(HttpStatusCode.OK, "Get Site counter", typeof(double));
        }

        private static async Task<double> Count(int key, EntityServiceKeyInt<Site> srv){
            var ret = await srv.Get(key);

            if (ret == null) {
                return 0.001;
            }

            return ret.Counter;
        }

    }
}
