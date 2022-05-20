﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.OData.Routing;
using Swashbuckle.Swagger.Annotations;
using static ODataRuntime.Builders.Helpers.AttributeHelper;

namespace ODataRuntime.Builders {
    public abstract class ActionBuilder {
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

        protected ActionBuilder(ControllerBuilder controllerBuilder, string actionName, Type returnType, params Type[] parameters) {
            MethodBuilder = controllerBuilder.CreateActionBuilder(actionName, returnType, parameters);
        }

        public ActionBuilder AddHttpVerb(HttpMethod method) {
            if (method == HttpMethod.Get) {
                MethodBuilder.SetCustomAttribute(CreateAttribute<HttpGetAttribute>());
            } else if (method == HttpMethod.Post) {
                MethodBuilder.SetCustomAttribute(CreateAttribute<HttpPostAttribute>());
            } else if (method == HttpMethod.Put) {
                MethodBuilder.SetCustomAttribute(CreateAttribute<HttpPutAttribute>());
            } else if (method == HttpMethod.Delete) {
                MethodBuilder.SetCustomAttribute(CreateAttribute<HttpDeleteAttribute>());
            }

            return this;
        }

        public ActionBuilder SetODataRoute(string route) {
            MethodBuilder.SetCustomAttribute(CreateAttribute<ODataRouteAttribute>(route));

            return this;
        }

        public ActionBuilder AddSwaggerResponse(HttpStatusCode status, string description = null, Type returnType = null) {
            MethodBuilder.SetCustomAttribute(CreateAttribute<SwaggerResponseAttribute>(status, description, returnType));

            return this;
        }

        public ActionBuilder SetResponseType(Type type) {
            MethodBuilder.SetCustomAttribute(CreateAttribute<ResponseTypeAttribute>(type));

            return this;
        }

        protected static int AddDelegate(Delegate @delegate) {
            lock (_DelegateLock) {
                _Delegates.Add(@delegate);
                return _Delegates.Count - 1;
            }
        }
    }
}