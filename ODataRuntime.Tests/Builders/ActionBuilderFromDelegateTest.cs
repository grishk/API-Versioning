using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ODataRuntime.Builders;

namespace ODataRuntime.Tests.Builders {
    public class ActionBuilderFromDelegateTest {
        [Test]
        public void AddMethodFromDelegateTest() {
            const string actionName = "GetTrue";

            string name = ReflectionHelper.GetUniqueAssemblyName(MethodBase.GetCurrentMethod());
            var assemblyBuilder = new AssemblyBuilder(name);

            Func<int, bool> isTrue = (i) => true;

            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, name, typeof(ActionBuilderFromBaseMethodTest.ParentClassStub))) {
                _ = new ActionBuilderFromDelegate(controllerBuilder, actionName, isTrue);
            }

            Type controllerType = ReflectionHelper.FindCreatedType(name);
            MethodInfo methodInfo = ReflectionHelper.FindMethodInfo(controllerType, actionName);
            MethodInfo delegateMethodInfo = isTrue.GetMethodInfo();

            Assert.IsNotNull(methodInfo);
            Assert.IsTrue(methodInfo.ReturnType == delegateMethodInfo.ReturnType);
            Assert.IsTrue(methodInfo.GetParameters().SequenceEqual(delegateMethodInfo.GetParameters(), new ActionBuilderFromBaseMethodTest.ParameterInfoComparer()));
        }
    }
}
