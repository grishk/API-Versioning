using Microsoft.AspNet.OData.Builder;
using Microsoft.Web.Http;

namespace ODataRuntime.Impl.Models
{
    public class ImplModelConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix)
        {
            builder.EntitySet<Client>(nameof(Client)).EntityType.HasKey(o => o.Key);
            builder.EntitySet<Site>(nameof(Site)).EntityType.HasKey(o => o.Key);
            builder.Function("GetClientFee").Returns<decimal>().Parameter<int>("ClientId");
        }
    }
}
