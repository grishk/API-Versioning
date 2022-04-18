using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Extra;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SeparateControllers.Models.DynamicAssembly
{
    public abstract class DynamicControllerBulder
    {
        protected readonly ModuleBuilder _moduleBuilder;
        protected readonly string _moduleName;
        protected readonly static ConstructorInfo VersionConstructor = typeof(ApiVersionAttribute).GetConstructor(new[] { typeof(string) });
        protected readonly static ConstructorInfo OdataRouteConstructor = typeof(ODataRoutePrefixAttribute).GetConstructor(new[] { typeof(string) });

        public DynamicControllerBulder(string name) 
        {
            _moduleName = $"{GetType().Assembly.GetName().Name}.{name}";
            var assemblyName = new AssemblyName(_moduleName);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule(_moduleName + ".dll", true);
        }

        public Type AddController(DynamicControllerDescription descriptions) 
        {
            Type baseType = descriptions.ParentType;
            TypeBuilder typeBuilder = _moduleBuilder
                .DefineType($"{_moduleName}.{descriptions.Name}Controller", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);
            typeBuilder.SetParent(baseType);

            if (descriptions.Versions != null && descriptions.Versions.Length > 0) 
            {
                foreach (var version in descriptions.Versions) 
                {
                    typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(VersionConstructor, new object[] { version }));
                }
            }

            if (!string.IsNullOrEmpty(descriptions.ODataPrefix))
            {
                 typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(OdataRouteConstructor, new object[] { descriptions.ODataPrefix }));
            }

            var createdType = typeBuilder.CreateType();

            return createdType;
        }

        public abstract void Build();
    }
}
