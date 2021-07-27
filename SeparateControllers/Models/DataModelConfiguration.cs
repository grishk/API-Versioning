using Microsoft.AspNet.OData.Builder;
using Microsoft.Web.Http;

namespace SeparateControllers.Models
{
    public class DataModelConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix)
        {
            builder.EntitySet<Person>("people").EntityType.HasKey(p => p.Id);
        }
        
    }
}