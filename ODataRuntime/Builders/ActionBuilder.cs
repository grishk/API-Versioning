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

        protected static CustomAttributeBuilder CreateAttribute<T>(params object[] args)
        {
            ConstructorInfo constructor = typeof(T).GetConstructor(args.Select(x => x.GetType()).ToArray());
            return new CustomAttributeBuilder(constructor, args);
        }


        protected MethodBuilder _methodBuilder;

        protected ActionBuilder() { }

        public ActionBuilder(ControllerBuilder controllerBuilder, string actioName, Type returnType, params Type[] parameters) 
        {
            _methodBuilder = controllerBuilder.CreateActionBuilder(actioName, returnType, parameters);
        }

        public ActionBuilder AddHttpVerb(HttpMethod method)
        {
            if (method == HttpMethod.Get)
                _methodBuilder.SetCustomAttribute(CreateAttribute<HttpGetAttribute>());
            else if (method == HttpMethod.Post)
                _methodBuilder.SetCustomAttribute(CreateAttribute< HttpPostAttribute>());
            else if (method == HttpMethod.Put)
                _methodBuilder.SetCustomAttribute(CreateAttribute<HttpPutAttribute>());
            else if (method == HttpMethod.Delete)
                _methodBuilder.SetCustomAttribute(CreateAttribute<HttpDeleteAttribute>());

            return this;
        }

        public ActionBuilder AddODataRoute(string route)
        {
            _methodBuilder.SetCustomAttribute(CreateAttribute<ODataRouteAttribute>(route));

            return this;
        }

        public ActionBuilder AddSwaggerResponse(HttpStatusCode status, string description = null, Type returnType = null)
        {
            _methodBuilder.SetCustomAttribute(CreateAttribute<SwaggerResponseAttribute>(                
                new object[] { status, description, returnType }));

            return this;
        }

        public ActionBuilder AddResponseType(Type type)
        {
            _methodBuilder.SetCustomAttribute(CreateAttribute<ResponseTypeAttribute>(type));

            return this;
        }

        public ActionBuilder AddParameter(int order, string name)
        {
            _methodBuilder.DefineParameter(order, ParameterAttributes.In, name);

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
