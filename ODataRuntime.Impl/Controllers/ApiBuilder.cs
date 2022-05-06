using ODataRuntime.Builders;

namespace ODataRuntime.Impl.Controllers
{
    public class ApiBuilder
    {
        

        public static void Register(ApiFactory apiFactory) 
        {
            AssemblyBuilder assemblyBuilder = new AssemblyBuilder("Test");
            apiFactory.Add(new ClientApi(assemblyBuilder));
            apiFactory.Add(new ClientFeeApi(assemblyBuilder));
            apiFactory.Add(new SiteApi(assemblyBuilder));
        }
    }

    public class ApiFactory
    {
        
        public void Add(Api api)
        {
            api.Register();
        }
    }
}
