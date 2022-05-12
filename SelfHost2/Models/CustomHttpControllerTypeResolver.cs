using SeparateControllers.DynamicBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace SelfHost2.Models
{
    /// <summary>
    /// a difference from DefaultHttpControllerTypeResolver only in CanExplore(assembly) 
    /// assembly.FullName.Contains(AssemblyBuilder.DynamicAssemblyBuilderName) check added
    /// </summary>
    public class CustomHttpControllerTypeResolver : DefaultHttpControllerTypeResolver
    {
        private readonly Func<Assembly, Type[]> _GetTypesFunc = GetTypes;

        /// <summary>
        /// CanExplore(assembly) added
        /// </summary>
        /// <param name="assembliesResolver"></param>
        /// <returns></returns>
        public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            List<Type> result = new List<Type>();

            // Go through all assemblies referenced by the application and search for types matching a predicate
            ICollection<Assembly> assemblies = assembliesResolver.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] exportedTypes = null;
                if (!CanExplore(assembly))
                {
                    // can't call GetTypes on a null (or dynamic?) assembly
                    continue;
                }

                try
                {
                    exportedTypes = _GetTypesFunc(assembly);
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

        private static bool CanExplore(Assembly assembly)
        {
            return assembly != null &&
                (!assembly.IsDynamic || assembly.FullName.Contains(AssemblyBuilder.DynamicAssemblyBuilderName));
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
