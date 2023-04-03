using Asp.Versioning;
using Asp.Versioning.OData;
using Microsoft.AspNet.OData.Builder;
using SeparateControllers.Extra;

namespace SeparateControllers.Models
{
	public class AddressesConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix)
        {
            builder.EntitySet<Address>("Addresses").EntityType.HasKey(o => o.Id);
        }
    }
}