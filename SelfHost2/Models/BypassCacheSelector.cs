using Microsoft.Web.Http;
using Microsoft.Web.Http.Dispatcher;
using Microsoft.Web.Http.Versioning;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace SelfHost2.Models
{
    public class ThruApiVersionControllerSelector : ApiVersionControllerSelector
    {
        HttpConfiguration config;
        public ThruApiVersionControllerSelector(HttpConfiguration configuration, ApiVersioningOptions options)
                : base(configuration, options)
        {
            var cm = this.GetControllerMapping();
            config = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            //var tt = InitializeCache();
            return base.SelectController(request);
        }

        string GetControllerName(Type type)
        {
            var name = type.GetCustomAttribute<ControllerNameAttribute>(false) is ControllerNameAttribute attribute ?
                attribute.Name :
                type.Name;

            return GroupName(NormalizeName(name));
        }

        System.Collections.Generic.Dictionary<string, ILookup<string, Type>> InitializeCache()
        {
            var services = this.config.Services;
            var assembliesResolver = services.GetAssembliesResolver();
            var typeResolver = services.GetHttpControllerTypeResolver();
            var comparer = StringComparer.OrdinalIgnoreCase;

            return typeResolver.GetControllerTypes(assembliesResolver)
                               .GroupBy(type => GetControllerName(type), comparer)
                               .ToDictionary(g => g.Key, g => g.ToLookup(t => t.Namespace ?? string.Empty, comparer), comparer);
        }

        /// <inheritdoc />
        public virtual string GroupName(string controllerName) => controllerName;

        public virtual string NormalizeName(string controllerName)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                return controllerName;
            }

            var length = controllerName.Length;
            var suffixLength = DefaultHttpControllerSelector.ControllerSuffix.Length;

            if (length <= suffixLength || !controllerName.EndsWith(DefaultHttpControllerSelector.ControllerSuffix, StringComparison.Ordinal))
            {
                return controllerName;
            }

            return controllerName.Substring(0, length - suffixLength);
        }
    }

    public class BypassCacheSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;

        public BypassCacheSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var controllerName = base.GetControllerName(request);
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes(); //GetExportedTypes doesn't work with dynamic assemblies
                var matchedType = types
                    .Where(i => typeof(IHttpController).IsAssignableFrom(i))
                    .Where(i => i.Name.ToLower() == controllerName.ToLower() + "controller")
                    .FirstOrDefault();

                if (matchedType != null)
                {
                    return new HttpControllerDescriptor(_configuration, controllerName, matchedType);
                }
            }

            return null;
        }
    }
}
