using System;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;
using ODataRuntime.Services;
using Owin;

namespace SelfHost2.Models {
    /// <summary>
    /// configure Autofac
    /// </summary>
    public static class AutofacApiBuilderExtension {
        /// <summary>
        /// configure Autofac DI
        /// </summary>
        /// <param name="apiBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IAppBuilder UseAutofac(this IAppBuilder apiBuilder, HttpConfiguration configuration) {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterApiControllers(AppDomain.CurrentDomain.GetAssemblies());
            containerBuilder.RegisterDomainServices();

            IContainer container = containerBuilder.Build();

            apiBuilder.UseAutofacWebApi(configuration);
            apiBuilder.UseAutofacMiddleware(container);
            return apiBuilder;
        }

        /// <summary>
        /// add Domain services
        /// </summary>
        /// <param name="containerBuilder"></param>
        private static void RegisterDomainServices(this ContainerBuilder containerBuilder) {
            containerBuilder.RegisterType<EntityServiceKeyInt<Client>>()
                            .As<IEntityService<int, Client>>()
                            .SingleInstance();
            containerBuilder.RegisterType<EntityServiceKeyInt<Site>>()
                            .As<IEntityService<int, Site>>()
                            .SingleInstance();
            containerBuilder.RegisterType<EntityServiceKeyInt<Manager>>()
                            .As<IEntityService<int, Manager>>()
                            .SingleInstance();
        }
    }
}