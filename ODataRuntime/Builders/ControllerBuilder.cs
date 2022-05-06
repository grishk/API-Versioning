using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ODataRuntime.Builders
{
    public class ControllerBuilder: IDisposable
    {
        private const string ControllerSufix = ".EControllers";

        private readonly static ConstructorInfo VersionConstructor = typeof(ApiVersionAttribute).GetConstructor(new[] { typeof(string) });
        private readonly static ConstructorInfo VersionNeutralConstructor = typeof(ApiVersionNeutralAttribute).GetConstructor(new Type[0]);
        private readonly static ConstructorInfo ODataRoutePrefixConstructor = typeof(ODataRoutePrefixAttribute).GetConstructor(new[] { typeof(string) });

        protected readonly TypeBuilder _typeBuilder;
        public Type BaseControllerType { get; }

        public ControllerBuilder(AssemblyBuilder assemblyBuilder, string controllerName, Type baseType) 
        {
            _typeBuilder = assemblyBuilder
                .CreateTypeBuilder($"{ControllerSufix}.{controllerName}Controller",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);

            BaseControllerType = baseType;
            _typeBuilder.SetParent(BaseControllerType);
        }

        public MethodBuilder CreateActionBuilder(string actioName, Type returnType, params Type[] parameters)
        {
            return _typeBuilder.DefineMethod(actioName,
                                             MethodAttributes.Public | MethodAttributes.Virtual,
                                             returnType,
                                             parameters);
        }

        public ControllerBuilder AddVersion( params string[] versions)
        {
            if (versions != null)
            {
                foreach (var version in versions)
                {
                    _typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(VersionConstructor, new object[] { version }));
                }
            }

            return this;
        }

        public ControllerBuilder  AddVersionNeutral()
        {
            _typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(VersionNeutralConstructor, new object[0]));
            return this;
        }

        public ControllerBuilder AddODataRoutePrefix(string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                _typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(ODataRoutePrefixConstructor, new object[] { prefix }));
            }

            return this;
        }

        public void Dispose()
        {
            _typeBuilder.CreateType();
        }
    }
}
