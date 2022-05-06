using Microsoft.OData;
using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;
using ODataRuntime.Services;
using System.Web.Http.Controllers;
using static Microsoft.OData.ServiceLifetime;

namespace ODataRuntime.Impl.Services
{
    public static class ServicesDomainExtension
    {
        public static IContainerBuilder AddDomainServices(this IContainerBuilder containerBuilder) 
        {
            containerBuilder.AddService<IEntityService<int, Client>>(Singleton, sp => new EntityServiceKeyInt<Client>());
            containerBuilder.AddService<IEntityService<int, Site>>(Singleton, sp => new EntityServiceKeyInt<Site>());

            return containerBuilder;
        }
    }
}
