using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.OData.Routing;
using NUnit.Framework;
using ODataRuntime.Builders;
using Swashbuckle.Swagger.Annotations;

namespace ODataRuntime.Tests.Builders {
    public class ActionBuilderFromBaseMethodTest {
        [Test]
        public void AddChildMethodFromParentNonPublicMethodTest() {
            const string actionName = "GetChildName";

            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(ParentClassStub))) {
                _ = new ActionBuilderFromBaseMethod(controllerBuilder, actionName, ParentClassStub.GetOneMethodName);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            MethodInfo methodInfo = ReflectionHelper.FindMethodInfo(controllerType, actionName);
            MethodInfo baseMethodInfo = ReflectionHelper.FindMethodInfo(typeof(ParentClassStub), ParentClassStub.GetOneMethodName);
            Assert.IsNotNull(methodInfo);
            Assert.IsTrue(methodInfo.ReturnType == baseMethodInfo.ReturnType);
            Assert.IsTrue(methodInfo.GetParameters().SequenceEqual(baseMethodInfo.GetParameters(), new ParameterInfoComparer()));
        }

        [Test]
        public void AddHttpVerbGetAttributeTest() {
            const string actionName = "GetChildName";

            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(ParentClassStub))) {
                var actionBuilder = new ActionBuilderFromBaseMethod(controllerBuilder, actionName, ParentClassStub.GetOneMethodName);
                actionBuilder.AddHttpVerb(HttpMethod.Get);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            MethodInfo methodInfo = ReflectionHelper.FindMethodInfo(controllerType, actionName);
            HttpGetAttribute[] attrs = ReflectionHelper.FindAttributeList<HttpGetAttribute>(methodInfo);

            Assert.IsTrue(attrs.Length == 1);
            Assert.IsTrue(attrs[0].HttpMethods.SequenceEqual(new []{ HttpMethod.Get }));
        }
        
        [Test]
        public void SetODataRouteAttributeTest() {
            const string actionName = "GetChildName";
            const string routeValue = "routeValue";

            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(ParentClassStub))) {
                var actionBuilder = new ActionBuilderFromBaseMethod(controllerBuilder, actionName, ParentClassStub.GetOneMethodName);
                actionBuilder.SetODataRoute(routeValue);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            MethodInfo methodInfo = ReflectionHelper.FindMethodInfo(controllerType, actionName);
            ODataRouteAttribute[] attrs = ReflectionHelper.FindAttributeList<ODataRouteAttribute>(methodInfo);

            Assert.IsTrue(attrs.Select(a=>a.PathTemplate).SequenceEqual(new[] { routeValue }));
        }

        [Test]
        public void SetResponseTypeAttributeTest() {
            const string actionName = "GetChildName";

            Type responseType = typeof(string);
            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(ParentClassStub))) {
                var actionBuilder = new ActionBuilderFromBaseMethod(controllerBuilder, actionName, ParentClassStub.GetOneMethodName);
                actionBuilder.SetResponseType(responseType);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            MethodInfo methodInfo = ReflectionHelper.FindMethodInfo(controllerType, actionName);
            ResponseTypeAttribute[] attrs = ReflectionHelper.FindAttributeList<ResponseTypeAttribute>(methodInfo);

            Assert.IsTrue(attrs.Select(a => a.ResponseType).SequenceEqual(new[] { responseType }));
        }

        [Test]
        public void AddSwaggerResponseAttributeTest() {
            const string actionName = "GetChildName";
            const string description = "Description";

            Type responseType = typeof(string);
            HttpStatusCode responseCode = HttpStatusCode.OK;
            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(ParentClassStub))) {
                var actionBuilder = new ActionBuilderFromBaseMethod(controllerBuilder, actionName, ParentClassStub.GetOneMethodName);
                actionBuilder.AddSwaggerResponse(responseCode, description, responseType);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            MethodInfo methodInfo = ReflectionHelper.FindMethodInfo(controllerType, actionName);
            SwaggerResponseAttribute[] attrs = ReflectionHelper.FindAttributeList<SwaggerResponseAttribute>(methodInfo);

            Assert.IsTrue(attrs.Select(a => a.StatusCode).SequenceEqual(new[] { (int)responseCode }));
            Assert.IsTrue(attrs.Select(a => a.Description).SequenceEqual(new[] { description }));
            Assert.IsTrue(attrs.Select(a => a.Type).SequenceEqual(new[] { responseType }));
        }

        /// <summary>
        /// Stub class to be parent
        /// </summary>
        public class ParentClassStub {
            public static string GetOneMethodName => nameof(GetName);

            private string GetName(int i, string s) {
                return $"{i} {s}";
            }
        }

        public class ParameterInfoComparer : IEqualityComparer<ParameterInfo> {
            public bool Equals(ParameterInfo x, ParameterInfo y) {
                return x?.Name == y?.Name && x?.ParameterType == y?.ParameterType;
            }

            public int GetHashCode(ParameterInfo obj) {
                return obj.GetHashCode();
            }
        }
    }
}