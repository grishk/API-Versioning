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
        private const string _ControllerSufix = ".EControllers";

        private readonly static ConstructorInfo _VersionConstructor = typeof(ApiVersionAttribute).GetConstructor(new[] { typeof(string) });
        private readonly static ConstructorInfo _VersionNeutralConstructor = typeof(ApiVersionNeutralAttribute).GetConstructor(new Type[0]);
        private readonly static ConstructorInfo _ODataRoutePrefixConstructor = typeof(ODataRoutePrefixAttribute).GetConstructor(new[] { typeof(string) });

        public PropertyInfo ServiceProperty { get; }

        protected readonly TypeBuilder _typeBuilder;
        public Type BaseControllerType { get; }

        public ControllerBuilder(AssemblyBuilder assemblyBuilder, string controllerName, Type baseType) 
        {
            _typeBuilder = assemblyBuilder
                .CreateTypeBuilder($"{_ControllerSufix}.{controllerName}Controller",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);

            BaseControllerType = baseType;
            _typeBuilder.SetParent(BaseControllerType);
            _typeBuilder.CreatePassThroughConstructors(BaseControllerType);
            ServiceProperty = baseType.GetProperty("Service", BindingFlags.NonPublic | BindingFlags.Instance);
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
                    _typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(_VersionConstructor, new object[] { version }));
                }
            }

            return this;
        }

        public ControllerBuilder  AddVersionNeutral()
        {
            _typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(_VersionNeutralConstructor, new object[0]));
            return this;
        }

        public ControllerBuilder AddODataRoutePrefix(string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                _typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(_ODataRoutePrefixConstructor, new object[] { prefix }));
            }

            return this;
        }

        public void Dispose()
        {
            _typeBuilder.CreateType();
        }
    }
}
