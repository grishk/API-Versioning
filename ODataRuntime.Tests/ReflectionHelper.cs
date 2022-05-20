using System;
using System.Collections.Generic;
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

        public static TAttr[] FindAttributeList<TAttr>(MemberInfo type) =>
            type.GetCustomAttributes(typeof(TAttr), false).OfType<TAttr>().ToArray();

        public static MethodInfo FindMethodInfo(Type type, string methodName) =>
            type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        public static bool SequenceEqual<T, TComparer>(this IEnumerable<T> first, IEnumerable<T> second)
            where TComparer : IEqualityComparer<T>, new()  =>
            first.SequenceEqual(second, new TComparer());

        public static string GetUniqueAssemblyName(MethodBase methodBase) {
            const string assemblyPrefix = "A_";

            return methodBase?.Name ?? $"{assemblyPrefix}{Guid.NewGuid()}";
        }

    }
}