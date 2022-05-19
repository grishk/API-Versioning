using System;
using System.Linq;
using System.Reflection;

namespace ODataRuntime.Tests {
    public static class ReflectionHelper {
        public static TypeInfo FindCreatedType(string name) {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = assemblies.SingleOrDefault(a => a.FullName.Contains((name)));

            if (assembly == null) {
                throw new NullReferenceException($"An assembly '{name}' does not found!");
            }

            return assembly.DefinedTypes.SingleOrDefault(t => t.Name.Contains(name));
        }

        public static TAttr[] FindAttributeList<TAttr>(Type type) =>
            type.GetCustomAttributes(typeof(TAttr), false).OfType<TAttr>().ToArray();
    }
}