using System;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.OData;
using Microsoft.OData.UriParser;
using Microsoft.Web.Http.Versioning;
using Microsoft.Web.Http.Versioning.Conventions;
using Owin;
using SeparateControllers.Models;
using Swashbuckle.Application;
using Swashbuckle.OData;
using static Microsoft.AspNet.OData.Query.AllowedQueryOptions;
using static Microsoft.OData.ODataUrlKeyDelimiter;
using static Microsoft.OData.ServiceLifetime;

namespace SelfHost
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            var httpServer = new HttpServer(config);
            
            // Web API configuration and services
            config.AddApiVersioning(
                cfg =>
                {
                    //cfg.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new UrlSegmentApiVersionReader());
                    //cfg.Conventions.Add(new VersionByNamespaceConvention());
                    cfg.ReportApiVersions = true;
                }
            );

            var modelBuilder = new VersionedODataModelBuilder(config)
            {
                ModelConfigurations =
                {
                    new DataModelConfiguration()
                }
            };


            var models = modelBuilder.GetEdmModels();
            config.Count();
            //config.MapVersionedODataRoute("odata-bypath", "odata/v{apiVersion}", models, ConfigureContainer);
            config.MapVersionedODataRoute("odata-bypath", "api", models, ConfigureContainer);
            
            var apiExplorer = config.AddODataApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            config.EnableSwagger(
                    "{apiVersion}/swagger",
                    swagger =>
                    {
                        swagger.MultipleApiVersions(
                            (apiDescription, version) => apiDescription.GetGroupName() == version,
                            info =>
                            {
                                foreach (var group in apiExplorer.ApiDescriptions)
                                {
                                    info.Version(group.Name, $"Sample API {group.ApiVersion}");
                                }
                            });
                    })
                .EnableSwaggerUi(swagger => swagger.EnableDiscoveryUrlSelector());

            appBuilder.UseWebApi(httpServer);
        }

        static void ConfigureContainer(IContainerBuilder builder)
        {
            builder.AddService<IODataPathHandler>(Singleton, sp => new DefaultODataPathHandler() { UrlKeyDelimiter = Parentheses });
            builder.AddService<ODataUriResolver>(Singleton, sp => new UnqualifiedCallAndEnumPrefixFreeResolver() { EnableCaseInsensitive = true });
        }


        public static string ContentRootPath
        {
            get
            {
                var app = AppDomain.CurrentDomain;

                if (string.IsNullOrEmpty(app.RelativeSearchPath))
                    return app.BaseDirectory;
                
                return app.RelativeSearchPath;
            }
        }
    }
}