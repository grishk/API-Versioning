using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using NUnit.Framework;
using ODataRuntime.Builders;

namespace ODataRuntime.Tests.Builders {
    [TestFixture]
    public class ControllerBuilderTest {
        [Test]
        public void ControllerTypeCreationTest() {
            string name = nameof(ControllerTypeCreationTest);
            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(name);

            using (new ControllerBuilder(assemblyBuilder, name, typeof(object)));

            Assert.IsNotNull(ReflectionHelper.FindCreatedType(name));
        }

        [Test]
        public void AddApiVersionTest() {
            const string apiVersion = "1";

            string name = nameof(AddApiVersionTest);
            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(name);

            using (ControllerBuilder controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
                controllerBuilder.AddVersion(apiVersion);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            ApiVersionAttribute attr = ReflectionHelper.FindAttributeList<ApiVersionAttribute>(controllerType)
                                                       .SingleOrDefault();
            Assert.IsNotNull(attr);
            Assert.IsTrue(attr.Versions.All(ver=>ver.ToString() == apiVersion));
        }

        [Test]
        public void AddApiVersionListTest() {
            const string apiVersion1 = "1";
            const string apiVersion2 = "2";

            string[] versionArray = { apiVersion1, apiVersion2 };
            string name = nameof(AddApiVersionListTest);
            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(name);

            using (ControllerBuilder controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
                controllerBuilder.AddVersion(apiVersion1);
                controllerBuilder.AddVersion(apiVersion2);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            ApiVersionAttribute[] attrs = ReflectionHelper.FindAttributeList<ApiVersionAttribute>(controllerType);
            Assert.IsTrue(attrs.All(attr => versionArray.Contains(attr.Versions[0].ToString())));
        }

        [Test]
        public void SetApiVersionNeutralTest() {
            string name = nameof(SetApiVersionNeutralTest);
            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(name);

            using (ControllerBuilder controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
                controllerBuilder.AddVersionNeutral();
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            ApiVersionNeutralAttribute attr = ReflectionHelper.FindAttributeList<ApiVersionNeutralAttribute>(controllerType)
                                                              .SingleOrDefault();
            Assert.IsNotNull(attr);
        }

        [Test]
        public void SetApiRouteTest() {
            const string apiRoute = "{key}";

            string name = nameof(SetApiRouteTest);
            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(name);

            using (ControllerBuilder controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(object))) {
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
