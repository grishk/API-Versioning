using System.Net;
using System.Net.Http;
using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;

namespace ODataRuntime.Impl.ApiControllers
{
    public class PeopleApi: ModelApi<Person>
    {
        public override void Register(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.AddODataRoutePrefix(nameof(Person));
            controllerBuilder.AddVersion("3");
            var actionBuilderGet = new ActionBuilderFromBaseMethod(controllerBuilder, "Get", "DoGet");
            actionBuilderGet
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(Person))
                .AddSwaggerResponse(HttpStatusCode.OK, "Client by Id", typeof(Client));
        }

        public PeopleApi(AssemblyBuilder assemblyBuilder) : base(assemblyBuilder) { }
    }
}