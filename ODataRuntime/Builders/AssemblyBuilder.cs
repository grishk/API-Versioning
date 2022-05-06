using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ODataRuntime.Builders
{
    public class AssemblyBuilder
    {
        public const string DynamicAssemblyBuilderName = "DynamicContollers";

        private ModuleBuilder _moduleBuilder;

        private readonly string _name;

        public AssemblyBuilder(string name)
        {
            _name = $"{DynamicAssemblyBuilderName}.{name}";
            Initialize();
        }

        protected void Initialize() 
        {
            var assemblyName = new AssemblyName(_name);
            System.Reflection.Emit.AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule(_name + ".dll", true);
        }

        public TypeBuilder CreateTypeBuilder(string sufixName, TypeAttributes attr) 
        {
            return _moduleBuilder.DefineType($"{_moduleBuilder.Assembly.GetName().Name}{sufixName}", attr);
        }

        public static AssemblyBuilder CreateAssebly(string name)
        {
            var ret = new AssemblyBuilder(name);
            ret.Initialize();
            return ret;
        }
    }
}
