using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;
using ODataRuntime.Services;

namespace ODataRuntime.Impl.Services
{
    public static class DomainServiceInit
    {
        public static void Initialize() 
        {
            ServiceContainer.Add<IEntityService<int, Client>>(new EntityServiceKeyInt<Client>());
            ServiceContainer.Add<IEntityService<int, Site>>(new EntityServiceKeyInt<Site>());
        }
    }
}
