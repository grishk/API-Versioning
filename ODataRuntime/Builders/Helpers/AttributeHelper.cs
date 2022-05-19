using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ODataRuntime.Builders.Helpers {
    public static class AttributeHelper {
        public static CustomAttributeBuilder CreateAttribute<T>(params object[] args) {
            ConstructorInfo constructor = typeof(T).GetConstructor(args.Select(x => x.GetType()).ToArray());
            return new CustomAttributeBuilder(constructor, args);
        }
    }
}