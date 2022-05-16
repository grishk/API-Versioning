using System.Net;
using System.Net.Http;
using ODataRuntime.Builders;
using ODataRuntime.Controllers;
using ODataRuntime.Models;

namespace ODataRuntime.Interfaces {
    public abstract class ModelApi<TModel> : Api
        where TModel : EntityKeyInt
    {
        protected ModelApi(AssemblyBuilder assemblyBuilder) : base(typeof(TModel).Name, typeof(BaseEntityODataControllerInt<TModel>), assemblyBuilder)
        {
        }

        protected void SetDefaultRoute() {
            ControllerBuilder.SetRoute(nameof(TModel));
        }

        protected virtual ActionBuilderFromBaseMethod RegisterGet() {
            var result = new ActionBuilderFromBaseMethod(ControllerBuilder, "Get", "DoGet");
            result
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(TModel))
                .AddSwaggerResponse(HttpStatusCode.OK, nameof(TModel) + " by Id", typeof(TModel));
            return result;
        }
    }
}