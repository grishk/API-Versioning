using SeparateControllers.Extra;

namespace SeparateControllers.Models.DynamicAssembly
{
    public class MarketControllerBuilder : DynamicControllerBulder
    {
        public MarketControllerBuilder()
            : base("DynamicTest") 
        { 
        }

        public override void Build()
        {
            var add = DynamicControllerDescription.Create<BaseODataController<LocalMarket>>(nameof(LocalMarket));
            add.Versions = new[] { "2", "3" };
            add.ODataPrefix = nameof(LocalMarket);
            AddController(add);

            add = DynamicControllerDescription.Create<BaseController<GlobalMarket>>(nameof(GlobalMarket));
            add.Versions = new[] { "2", "4" };
            //add.ODataPrefix = nameof(GlobalMarket);
            AddController(add);
        }
    }
}
