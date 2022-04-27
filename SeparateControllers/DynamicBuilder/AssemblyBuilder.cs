using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SeparateControllers.DynamicBuilder
{
    public class AssemblyBuilder
    {
        public const string DynamicAssemblyBuilderName = "DynamicContollers";

        private readonly string _name;

        AssemblyBuilder(string name)
        {
            _name = $"{DynamicAssemblyBuilderName}.{name}";
        }

        public static ModuleBuilder CreateAssebly(string name)
        {
            var assembly = new AssemblyBuilder(name);
            var assemblyName = new AssemblyName(assembly._name);
            System.Reflection.Emit.AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly._name + ".dll", true);
            return moduleBuilder;
        }
    }
}
