using Asp.Versioning;
using Asp.Versioning.OData;
using Microsoft.AspNet.OData.Builder;

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