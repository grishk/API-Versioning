using Asp.Versioning;
using Asp.Versioning.OData;
using Microsoft.AspNet.OData.Builder;

namespace SeparateControllers.Models
{
	public class AllConfigurations : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix)
        {
            builder.EntitySet<Person>("People").EntityType.HasKey(o => o.Id);
            builder.EntitySet<Order>("Orders").EntityType.HasKey(o => o.Id);
            builder.Function("Ping").Returns<int>().Parameter<int>("IP");
            var health = builder.Function("Health").Returns<string>();
            health.Parameter<int>("Version");
            health.Parameter<string>("Family");
            var healther = builder.Function("Healther").Returns<double>();
           // healther.Parameter<int>("Version");
            healther.Parameter<string>("Family");
            builder.Function("Pinging").Returns<int>().Parameter<int>("IP");
            builder.Function("GetSalesTaxRate").Returns<double>().Parameter<int>("PostalCode");
        }
    }
}