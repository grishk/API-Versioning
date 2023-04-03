using Asp.Versioning;
using Microsoft.AspNet.OData.Routing;
using NUnit.Framework;
using ODataRuntime.Builders;
using System;
using System.Linq;
using System.Reflection;

namespace ODataRuntime.Tests.Builders
{
	[TestFixture]
    public class ControllerBuilderTest {
        [Test]
        public void ControllerTypeCreationTest() {
            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
                ;
            }

            Assert.IsNotNull(ReflectionHelper.FindCreatedType(name));
        }

        [Test]
        public void AddApiVersionAttributeTest() {
            const string apiVersion = "1";

            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
                controllerBuilder.AddVersion(apiVersion);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            ApiVersionAttribute attr = ReflectionHelper.FindAttributeList<ApiVersionAttribute>(controllerType)
                                                       .SingleOrDefault();
            Assert.IsNotNull(attr);
            Assert.IsTrue(attr.Versions.All(ver => ver.ToString() == apiVersion));
        }

        [Test]
        public void AddApiVersionAttributeListTest() {
            const string apiVersion1 = "1";
            const string apiVersion2 = "2";

            string[] versionArray = { apiVersion1, apiVersion2 };
            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
                controllerBuilder.AddVersion(apiVersion1);
                controllerBuilder.AddVersion(apiVersion2);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            ApiVersionAttribute[] attrs = ReflectionHelper.FindAttributeList<ApiVersionAttribute>(controllerType);
            Assert.IsTrue(attrs.All(attr => versionArray.Contains(attr.Versions[0].ToString())));
        }

        [Test]
        public void AddApiVersionNeutralAttributeTest() {
            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
                controllerBuilder.AddVersionNeutral();
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            ApiVersionNeutralAttribute attr = ReflectionHelper.FindAttributeList<ApiVersionNeutralAttribute>(controllerType)
                                                              .SingleOrDefault();
            Assert.IsNotNull(attr);
        }

        [Test]
        public void SetApiRouteAttributeTest() {
            const string apiRoute = "{key}";

            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
                controllerBuilder.SetRoute(apiRoute);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            ODataRoutePrefixAttribute attr = ReflectionHelper.FindAttributeList<ODataRoutePrefixAttribute>(controllerType)
                                                             .SingleOrDefault();
            Assert.IsNotNull(attr);
            Assert.IsTrue(attr.Prefix == apiRoute);
        }
    }
}