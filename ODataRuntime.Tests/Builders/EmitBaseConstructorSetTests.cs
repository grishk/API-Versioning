using System;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using ODataRuntime.Builders.Helpers;

namespace ODataRuntime.Tests.Builders {
    [TestFixture]
    public class DynamicTypeConstructorTest {
        
        [Test]
        public void CreatedChildClassHasAllConstructorsFromBase() {
            TypeBuilder typeBuilder = InitRuntimeMockType();

            Type runtimeType = CreateChildType(typeBuilder, typeof(MockBase));
     
            Assert.IsNotNull(GetMockTypeConstructor(runtimeType));
            Assert.IsNotNull(GetMockTypeConstructor(runtimeType, typeof(int)));
            Assert.IsNotNull(GetMockTypeConstructor(runtimeType, typeof(string)));
            Assert.IsNotNull(GetMockTypeConstructor(runtimeType, typeof(int?)));
            Assert.IsNotNull(GetMockTypeConstructor(runtimeType, typeof(int), typeof(string), typeof(int?)));
            Assert.IsNull(GetMockTypeConstructor(runtimeType, typeof(decimal)));
        }

        private static ConstructorInfo GetMockTypeConstructor(Type type, params Type[] args) {
            return type.GetConstructor(args);
        }

        private static TypeBuilder InitRuntimeMockType() {
            const string name = "MockAssembly";
            const string objectName = "MockObject";

            var assemblyName = new AssemblyName(name);
            AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule($"{name}.dll", true);
            return moduleBuilder.DefineType(objectName, TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);
        }

        private static Type CreateChildType(TypeBuilder typeBuilder, Type parent) {
            typeBuilder.SetParent(parent);
            typeBuilder.CreatePassThroughConstructors(parent);
            Type runtimeType = typeBuilder.CreateType();
            return runtimeType;
        }

        public class MockBase {
            public MockBase() { }
            public MockBase(int i) { }
            public MockBase(string s) { }
            public MockBase(int? nullableI) { }
            public MockBase(int i, string s, int? nullableI) { }
        }
    }
}