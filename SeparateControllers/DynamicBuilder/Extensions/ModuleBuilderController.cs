using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SeparateControllers.DynamicBuilder.Extensions
{
    public static class ModuleBuilderController
    {
        private const string ControllerSufix = ".EControllers";

        public static TypeBuilder BuildControllerBegin<TBase>(this ModuleBuilder moduleBuilder, string name)
        {
            TypeBuilder typeBuilder = moduleBuilder
                .DefineType($"{moduleBuilder.Assembly.GetName().Name}{ControllerSufix}.{name}Controller", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);

            Type baseType = typeof(TBase);
            typeBuilder.SetParent(baseType);
            return typeBuilder;
        }

        public static ModuleBuilder BuildControllerEnd(this TypeBuilder typeBuilder)
        {
            typeBuilder.CreateType();
            return typeBuilder.Module as ModuleBuilder;
        }
    }
}