using System.Net;
using System.Net.Http;
using ODataRuntime.Builders;
using ODataRuntime.Models;

namespace ODataRuntime.Interfaces {
    public abstract class ModelApi<TModel> : BoundApi<int, TModel>
        where TModel: EntityKeyInt {
        protected void SetDefaultRoute() {
            ControllerBuilder.SetRoute(nameof(TModel));
        }

        protected virtual ActionBuilderFromBaseMethod RegisterGet() {
            var result = new ActionBuilderFromBaseMethod(ControllerBuilder, "Get", "DoGet");
            result
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(TModel))
                .AddSwaggerResponse(HttpStatusCode.OK, typeof(TModel).Name + " by Id", typeof(TModel));
            return result;
        }
    }
}