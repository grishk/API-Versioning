using Microsoft.AspNet.OData.Builder;
using Microsoft.Web.Http;
using SeparateControllers.Extra;

namespace SeparateControllers.Models
{
    public class AllConfigurations : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix)
        {
            builder.EntitySet<Person>("People").EntityType.HasKey(o => o.Id);
            builder.EntitySet<Order>("Orders").EntityType.HasKey(o => o.Id);
        }
    }
}