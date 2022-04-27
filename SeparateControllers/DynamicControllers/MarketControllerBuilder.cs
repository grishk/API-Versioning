using SeparateControllers.DynamicBuilder;
using SeparateControllers.DynamicBuilder.Extensions;
using SeparateControllers.Extra;
using SeparateControllers.Models;

namespace SeparateControllers.DynamicControllers
{
    public static class MarketControllerBuilder
    {
        public static void Build()
        {
            var builder = AssemblyBuilder.CreateAssebly("Market");
            builder.BuildControllerBegin<BaseODataController<LocalMarket>>(nameof(LocalMarket))
                .AddODataRoutePrefix(nameof(LocalMarket))
                .AddVersion("2", "3")
                .BuildControllerEnd()
                .BuildControllerBegin<BaseController<GlobalMarket>>(nameof(GlobalMarket))
                //.AddODataRoutePrefix(nameof(GlobalMarket))
                .AddVersion("2", "4")
                .BuildControllerEnd();
        }
    }
}
