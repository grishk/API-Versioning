using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using ODataRuntime.Builders.Helpers;
using static ODataRuntime.Builders.Helpers.AttributeHelper;

namespace ODataRuntime.Builders {
    public class ControllerBuilder : IDisposable {
        private const string _ControllerSuffix = ".EControllers";

        protected readonly TypeBuilder TypeBuilder;

        //private readonly static ConstructorInfo _VersionConstructor = typeof(ApiVersionAttribute).GetConstructor(new[] { typeof(string) });
        //private readonly static ConstructorInfo _VersionNeutralConstructor = typeof(ApiVersionNeutralAttribute).GetConstructor(new Type[0]);
        //private readonly static ConstructorInfo _ODataRoutePrefixConstructor = typeof(ODataRoutePrefixAttribute).GetConstructor(new[] { typeof(string) });

        public PropertyInfo ServiceProperty { get; }
        public Type BaseControllerType { get; }

        public ControllerBuilder(AssemblyBuilder assemblyBuilder, string controllerName, Type baseType) {
            TypeBuilder = assemblyBuilder
                .CreateTypeBuilder($"{_ControllerSuffix}.{controllerName}Controller",
                                   TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);

            BaseControllerType = baseType;
            TypeBuilder.SetParent(BaseControllerType);
            TypeBuilder.CreatePassThroughConstructors(BaseControllerType);
            ServiceProperty = baseType.GetProperty("Service", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public void Dispose() {
            TypeBuilder.CreateType();
        }

        public MethodBuilder CreateActionBuilder(string actioName, Type returnType, params Type[] parameters) {
            return TypeBuilder.DefineMethod(actioName,
                                            MethodAttributes.Public | MethodAttributes.Virtual,
                                            returnType,
                                            parameters);
        }

        public ControllerBuilder AddVersion(params string[] versions) {
            if (versions != null) {
                foreach (string version in versions) {
                    TypeBuilder.SetCustomAttribute(CreateAttribute<ApiVersionAttribute>(new object[] { version }));
                }
            }

            return this;
        }

        public ControllerBuilder AddVersionNeutral() {
            TypeBuilder.SetCustomAttribute(CreateAttribute<ApiVersionNeutralAttribute>());
            return this;
        }

        public ControllerBuilder SetRoute(string prefix) {
            if (!string.IsNullOrWhiteSpace(prefix)) {
                TypeBuilder.SetCustomAttribute(CreateAttribute<ODataRoutePrefixAttribute>(new object[] { prefix }));
            }

            return this;
        }
    }
}