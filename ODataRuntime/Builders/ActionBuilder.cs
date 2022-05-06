using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
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
        protected readonly static ConstructorInfo HttpGetConstructor = typeof(HttpGetAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly static ConstructorInfo HttpDeleteConstructor = typeof(HttpDeleteAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly static ConstructorInfo HttpPatchConstructor = typeof(HttpPatchAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly static ConstructorInfo HttpPutConstructor = typeof(HttpPutAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly static ConstructorInfo HttpPostConstructor = typeof(HttpPostAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly static ConstructorInfo ODataRouteConstructor = typeof(ODataRouteAttribute).GetConstructor(new[] { typeof(string) });
        protected readonly static ConstructorInfo SwaggerResponseConstructor = typeof(SwaggerResponseAttribute).GetConstructor(new[] { typeof(HttpStatusCode), typeof(string), typeof(Type) });
        protected readonly static ConstructorInfo ResponseTypeConstructor = typeof(ResponseTypeAttribute).GetConstructor(new[] { typeof(Type) });
        protected readonly static ConstructorInfo FromODataUriConstructor = typeof(FromODataUriAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly static ConstructorInfo SelectConstructor = typeof(EnableQueryAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly static PropertyInfo ModelBinderNameProperty = typeof(ModelBinderAttribute).GetProperty(nameof(ModelBinderAttribute.Name), BindingFlags.Public | BindingFlags.Instance);

        protected MethodBuilder _methodBuilder;

        protected ActionBuilder() { }

        public ActionBuilder(ControllerBuilder controllerBuilder, string actioName, Type returnType, params Type[] parameters) 
        {
            _methodBuilder = controllerBuilder.CreateActionBuilder(actioName, returnType, parameters);
        }

        public ActionBuilder AddHttpVerb(HttpMethod method)
        {
            if (method == HttpMethod.Get)
                _methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(HttpGetConstructor, new object[0]));
            else if (method == HttpMethod.Post)
                _methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(HttpPostConstructor, new object[0]));
            else if (method == HttpMethod.Put)
                _methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(HttpPutConstructor, new object[0]));
            else if (method == HttpMethod.Delete)
                _methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(HttpDeleteConstructor, new object[0]));

            return this;
        }

        public ActionBuilder AddODataRoute(string route)
        {
            _methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(ODataRouteConstructor, new object[] { route }));

            return this;
        }

        public ActionBuilder AddSwaggerResponse(HttpStatusCode status, string description = null, Type returnType = null)
        {
            _methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(SwaggerResponseConstructor,
                new object[] { status, description, returnType }));

            return this;
        }

        public ActionBuilder AddResponseType(Type type)
        {
            _methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(ResponseTypeConstructor, new object[] { type }));

            return this;
        }

        public ActionBuilder AddParameter(int order, string name)
        {
            _methodBuilder.DefineParameter(order, ParameterAttributes.In, name);

            return this;
        }
    }
}
