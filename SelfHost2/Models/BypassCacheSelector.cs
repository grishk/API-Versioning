using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace SelfHost2.Models
{
	public class BypassCacheSelector : DefaultHttpControllerSelector {
        private readonly HttpConfiguration _Configuration;

        public BypassCacheSelector(HttpConfiguration configuration)
            : base(configuration) {
            _Configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request) {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var controllerName = base.GetControllerName(request);
            foreach (var assembly in assemblies) {
                var types = assembly.GetTypes(); //GetExportedTypes doesn't work with dynamic assemblies
                var matchedType = types
                                  .Where(i => typeof(IHttpController).IsAssignableFrom(i))
                                  .Where(i => i.Name.ToLower() == controllerName.ToLower() + "controller")
                                  .FirstOrDefault();

                if (matchedType != null) {
                    return new HttpControllerDescriptor(_Configuration, controllerName, matchedType);
                }
            }

            return null;
        }
    }
}