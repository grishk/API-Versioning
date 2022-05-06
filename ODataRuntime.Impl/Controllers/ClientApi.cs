using System;
using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using System.Net;
using System.Net.Http;
using ODataRuntime.Controllers;
using ODataRuntime.Models;

namespace ODataRuntime.Impl.Controllers
{
    public class ClientApi : Api<Client>
    {
        public ClientApi(AssemblyBuilder assemblyBuilder):base(assemblyBuilder)
        {
        }

        protected override void Register(ControllerBuilder builder)
        {
            builder.AddODataRoutePrefix(nameof(Client));
            builder.AddVersion("0.3");
            var actionBuilderGet = new ActionBuilderFromBaseMethod(builder, "Get", "DoGet");
            actionBuilderGet
                .AddHttpVerb(HttpMethod.Get)
                .AddResponseType(typeof(Client))
                .AddSwaggerResponse(HttpStatusCode.OK, "Client by Id", typeof(Client));
        }
    }

    public abstract class Api<T> : Api, IDisposable where T : BaseEntity<int>
    {
        protected Api(AssemblyBuilder assemblyBuilder) : base(assemblyBuilder, nameof(T), typeof(BaseEntityODataControllerInt<T>))
        {
        }
    }

    public abstract class Api
    {
        private readonly ControllerBuilder _builder;
        protected Api(AssemblyBuilder assemblyBuilder, string name, Type baseType)
        {
            _builder = new ControllerBuilder(assemblyBuilder, name, baseType);
        }

        public void Register()
        {
            Register(_builder);
        }

        protected abstract void Register(ControllerBuilder builder);

        public void Dispose()
        {
            _builder?.Dispose();
        }
    }
}
