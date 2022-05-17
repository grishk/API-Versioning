using System;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using ODataRuntime.Builders.Helpers;

namespace ODataRuntime.Tests.Builders {
    [TestFixture]
    public class EmitBaseConstructorSetTests {
        [Test]
        public void EmitEmptyBaseConstructorTest() {
            Type runtimeType = CreateRuntimeMockType();

            ConstructorInfo constructorInfo = GetMockTypeConstructor(runtimeType);

            Assert.IsNotNull(constructorInfo);
        }

        [Test]
        public void EmitIntBaseConstructorTest() {
            Type runtimeType = CreateRuntimeMockType();

            ConstructorInfo constructorInfo = GetMockTypeConstructor(runtimeType, typeof(int));

            Assert.IsNotNull(constructorInfo);
        }

        [Test]
        public void EmitObjectBaseConstructorTest() {
            Type runtimeType = CreateRuntimeMockType();

            ConstructorInfo constructorInfo = GetMockTypeConstructor(runtimeType, typeof(string));

            Assert.IsNotNull(constructorInfo);
        }

        [Test]
        public void EmitNullableBaseConstructorTest() {
            Type runtimeType = CreateRuntimeMockType();

            ConstructorInfo constructorInfo = GetMockTypeConstructor(runtimeType, typeof(int?));

            Assert.IsNotNull(constructorInfo);
        }

        [Test]
        public void EmitMultiParamBaseConstructorTest() {
            Type runtimeType = CreateRuntimeMockType();

            ConstructorInfo constructorInfo = GetMockTypeConstructor(runtimeType, typeof(int), typeof(string), typeof(int?));

            Assert.IsNotNull(constructorInfo);
        }

        [Test]
        public void NotEmitDecimalBaseConstructorTest() {
            Type runtimeType = CreateRuntimeMockType();

            ConstructorInfo constructorInfo = GetMockTypeConstructor(runtimeType, typeof(decimal));

            Assert.IsNull(constructorInfo);
        }


        private static ConstructorInfo GetMockTypeConstructor(Type type, params Type[] args) {
            ConstructorInfo constructorInfo = type.GetConstructor(args);
            return constructorInfo;
        }

        private static Type CreateRuntimeMockType() {
            const string name = "MockAssembly";
            const string objectName = "MockObject";

            var assemblyName = new AssemblyName(name);
            AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule($"{name}.dll", true);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(objectName,
                                                               TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);
            Type baseType = typeof(MockBase);
            typeBuilder.SetParent(baseType);
            typeBuilder.CreatePassThroughConstructors(baseType);
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