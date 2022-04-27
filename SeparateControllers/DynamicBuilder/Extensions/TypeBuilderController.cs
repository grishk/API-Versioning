using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SeparateControllers.DynamicBuilder.Extensions
{
    public static class TypeBuilderController
    {
        private readonly static ConstructorInfo VersionConstructor = typeof(ApiVersionAttribute).GetConstructor(new[] { typeof(string) });
        private readonly static ConstructorInfo VersionNeutralConstructor = typeof(ApiVersionNeutralAttribute).GetConstructor(new Type[0]);
        private readonly static ConstructorInfo ODataRoutePrefixConstructor = typeof(ODataRoutePrefixAttribute).GetConstructor(new[] { typeof(string) });
        
        public static TypeBuilder AddVersion(this TypeBuilder typeBuilder, params string[] versions)
        {
            if (versions != null)
            {
                foreach (var version in versions)
                {
                    typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(VersionConstructor, new object[] { version }));
                }
            }

            return typeBuilder;
        }
        public static TypeBuilder AddVersionNeutral(this TypeBuilder typeBuilder)
        {
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(VersionNeutralConstructor, new object[0]));
            return typeBuilder;
        }

        public static TypeBuilder AddODataRoutePrefix(this TypeBuilder typeBuilder, string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(ODataRoutePrefixConstructor, new object[] { prefix }));
            }

            return typeBuilder;
        }

        public static MethodBuilder AddMethodBegin(this TypeBuilder typeBuilder, string name, Type returnType, params Type[] parameters)
        {
            var methodBuilder = typeBuilder.DefineMethod(
                                             name,
                                             MethodAttributes.Public |
                                             MethodAttributes.Virtual,
                                             returnType,
                                             parameters);

            return methodBuilder;
        }

        public static TypeBuilder AddMethodEnd(this MethodBuilder methodBuilder)
        {
            return methodBuilder.ReflectedType as TypeBuilder;
        }
    }
}