using SeparateControllers.Models.DynamicAssembly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace SelfHost2.Models
{
    public class CustomHttpControllerTypeResolver : DefaultHttpControllerTypeResolver
    {
        private Func<Assembly, Type[]> _getTypesFunc = GetTypes;

        public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            List<Type> result = new List<Type>();

            // Go through all assemblies referenced by the application and search for types matching a predicate
            ICollection<Assembly> assemblies = assembliesResolver.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] exportedTypes = null;
                if (assembly == null || 
                    assembly.IsDynamic && !assembly.FullName.Contains(DynamicControllerBulder.DynamicAssemblyBuilderName))
                {
                    // can't call GetTypes on a null (or dynamic?) assembly
                    continue;
                }

                try
                {
                    exportedTypes = _getTypesFunc(assembly);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exportedTypes = ex.Types;
                }
                catch
                {
                    // We deliberately ignore all exceptions when building the cache. If 
                    // a controller type is not found then we will respond later with a 404.
                    // However, until then we don't know whether an exception at all will
                    // have an impact on finding a controller.
                    continue;
                }

                if (exportedTypes != null)
                {
                    result.AddRange(exportedTypes.Where(x => TypeIsVisible(x) && IsControllerTypePredicate(x)));
                }
            }

            return result;
        }

        private static Type[] GetTypes(Assembly assembly)
        {
            return assembly.GetTypes();
        }

        private static bool TypeIsVisible(Type type)
        {
            return (type != null && type.IsVisible);
        }
    }
}
