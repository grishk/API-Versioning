using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ODataRuntime.Builders
{
    public class AssemblyBuilder
    {
        public const string DynamicAssemblyBuilderName = "DynamicContollers";

        private ModuleBuilder _ModuleBuilder;

        private readonly string _Name;

        public AssemblyBuilder(string name)
        {
            _Name = $"{DynamicAssemblyBuilderName}.{name}";
            Initialize();
        }

        protected void Initialize() 
        {
            var assemblyName = new AssemblyName(_Name);
            System.Reflection.Emit.AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            _ModuleBuilder = assemblyBuilder.DefineDynamicModule(_Name + ".dll", true);
        }

        public TypeBuilder CreateTypeBuilder(string sufixName, TypeAttributes attr) 
        {
            return _ModuleBuilder.DefineType($"{_ModuleBuilder.Assembly.GetName().Name}{sufixName}", attr);
        }

        public static AssemblyBuilder CreateAssebly(string name)
        {
            var ret = new AssemblyBuilder(name);
            ret.Initialize();
            return ret;
        }
    }
}
