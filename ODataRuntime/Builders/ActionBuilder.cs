using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using static ODataRuntime.Builders.AttributeHelper;

namespace ODataRuntime.Builders
{
    public abstract class ActionBuilder
    {
        protected static readonly List<Delegate> _Delegates = new List<Delegate>();
        private static readonly object _DelegateLock = new object();

        //protected readonly static ConstructorInfo ODataRouteConstructor1 = typeof(ODataRouteAttribute).GetConstructor(new[] { typeof(string) });
        //protected readonly static ConstructorInfo SwaggerResponseConstructor1 = typeof(SwaggerResponseAttribute).GetConstructor(new[] { typeof(HttpStatusCode), typeof(string), typeof(Type) });
        //protected readonly static ConstructorInfo ResponseTypeConstructor1 = typeof(ResponseTypeAttribute).GetConstructor(new[] { typeof(Type) });
        //protected readonly static ConstructorInfo FromODataUriConstructor1 = typeof(FromODataUriAttribute).GetConstructor(Type.EmptyTypes);
        //protected readonly static ConstructorInfo SelectConstructor = typeof(EnableQueryAttribute).GetConstructor(Type.EmptyTypes);
        //protected readonly static PropertyInfo ModelBinderNameProperty = typeof(ModelBinderAttribute).GetProperty(nameof(ModelBinderAttribute.Name), BindingFlags.Public | BindingFlags.Instance);

        protected MethodBuilder MethodBuilder;

        protected ActionBuilder() { }

        public ActionBuilder(ControllerBuilder controllerBuilder, string actioName, Type returnType, params Type[] parameters) 
        {
            MethodBuilder = controllerBuilder.CreateActionBuilder(actioName, returnType, parameters);
        }

        public ActionBuilder AddHttpVerb(HttpMethod method)
        {
            if (method == HttpMethod.Get)
                MethodBuilder.SetCustomAttribute(CreateAttribute<HttpGetAttribute>());
            else if (method == HttpMethod.Post)
                MethodBuilder.SetCustomAttribute(CreateAttribute< HttpPostAttribute>());
            else if (method == HttpMethod.Put)
                MethodBuilder.SetCustomAttribute(CreateAttribute<HttpPutAttribute>());
            else if (method == HttpMethod.Delete)
                MethodBuilder.SetCustomAttribute(CreateAttribute<HttpDeleteAttribute>());

            return this;
        }

        public ActionBuilder AddODataRoute(string route)
        {
            MethodBuilder.SetCustomAttribute(CreateAttribute<ODataRouteAttribute>(route));

            return this;
        }

        public ActionBuilder AddSwaggerResponse(HttpStatusCode status, string description = null, Type returnType = null)
        {
            MethodBuilder.SetCustomAttribute(CreateAttribute<SwaggerResponseAttribute>(                
                new object[] { status, description, returnType }));

            return this;
        }

        public ActionBuilder AddResponseType(Type type)
        {
            MethodBuilder.SetCustomAttribute(CreateAttribute<ResponseTypeAttribute>(type));

            return this;
        }

        public ActionBuilder AddParameter(int order, string name)
        {
            MethodBuilder.DefineParameter(order, ParameterAttributes.In, name);

            return this;
        }
        protected static int AddDelegate(Delegate dlgt) {
            lock (_DelegateLock) {
                _Delegates.Add(dlgt);
                return _Delegates.Count - 1;
            }
        }
    }
}
