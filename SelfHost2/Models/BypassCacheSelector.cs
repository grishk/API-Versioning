namespace SelfHost2
{
    using Microsoft.Web.Http.Dispatcher;
    using Microsoft.Web.Http.Versioning;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    public class BypassCacheSelector : ApiVersionControllerSelector
    {
        private readonly HttpConfiguration _configuration;

        public BypassCacheSelector(HttpConfiguration configuration, ApiVersioningOptions options)
            : base(configuration, options)
        {
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            try
            {
                return base.SelectController(request);
            }
            catch 
            { 
            }
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