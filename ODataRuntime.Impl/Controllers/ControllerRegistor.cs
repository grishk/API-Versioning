using Microsoft.AspNet.OData;
using ODataRuntime.Builders;
using ODataRuntime.Controllers;
using ODataRuntime.Impl.Models;

namespace ODataRuntime.Impl.Controllers
{
    public static partial class ControllerRegistor
    {
        public static void Register() 
        {
            var assemblyBuilder = new AssemblyBuilder("Test");

            using (var controllerBuilderClient = 
                new ControllerBuilder(assemblyBuilder, nameof(Client), typeof(BaseEntityODataControllerInt<Client>))) 
            {
                RegisterClient(controllerBuilderClient);
            }

            using (var controllerBuilderSite =
                new ControllerBuilder(assemblyBuilder, nameof(Site), typeof(BaseEntityODataControllerInt<Site>)))
            {
                RegisterSite(controllerBuilderSite);
            }

            using (var controllerBuilderFunc =
                new ControllerBuilder(assemblyBuilder, "Fees", typeof(ODataController)))
            {
                RegisterFeeFunc(controllerBuilderFunc);
            }
        }
    }
}
