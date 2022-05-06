using SeparateControllers.Models;
using Swashbuckle.Application;

namespace SelfHost2
{
    using global::Owin;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.OData;
    using Microsoft.OData.Edm;
    using Microsoft.OData.UriParser;
    using Microsoft.Web.Http;
    using Newtonsoft.Json.Serialization;
    using ODataRuntime.Impl.Controllers;
    using ODataRuntime.Impl.Models;
    using ODataRuntime.Impl.Services;
    using SelfHost2.Models;
    using SeparateControllers.DynamicControllers;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.ExceptionHandling;
    using static Microsoft.AspNet.OData.Query.AllowedQueryOptions;
    using static Microsoft.OData.ODataUrlKeyDelimiter;
    using static Microsoft.OData.ServiceLifetime;

    /// <summary>
    /// Represents the startup process for the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the application using the provided builder.
        /// </summary>
        /// <param name="builder">The current application builder.</param>
        public void Configuration(IAppBuilder builder)
        {
            var configuration = new HttpConfiguration();

            configuration.Routes.MapHttpRoute(
                           name: "DefaultApi",
                           routeTemplate: "api/{controller}/{action}/{id}",
                           defaults: new { id = RouteParameter.Optional }
                       );

            // create dynamic controllers
            MarketControllerBuilder.Build();
            PingControllerBuilder.Build();

            ApiBuilder.Register();
            DomainServiceInit.Initialize();

            // handling arror
            configuration.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            // include dynamic assemblies
            configuration.Services.Replace(typeof(IHttpControllerTypeResolver), new CustomHttpControllerTypeResolver());

            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            Microsoft.Web.Http.Versioning.ApiVersioningOptions opts = null;
            configuration.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                opts = options;
            });

            // note: this is required to make the default swagger json settings match the odata conventions applied by EnableLowerCamelCase()
            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var modelBuilder = new VersionedODataModelBuilder(configuration)
            {
                ModelConfigurations =
                {
                    new AllConfigurations(),
                    new AddressesConfiguration(),
                    new DynamicConfiguration(),
                    new ImplModelConfiguration()
                    //new PersonModelConfiguration(),
                    //new OrderModelConfiguration(),
                    //new ProductConfiguration(),
                    //new SupplierConfiguration(),
                }
            };
            var models = modelBuilder.GetEdmModels();

            //           model.VocabularyAnnotations.Add(new ApiVersionAttribute("1.0"));
            // global odata query options
            configuration.Count();

            // INFO: while you can use both, you should choose only ONE of the following; comment, uncomment, or remove as necessary

            // WHEN VERSIONING BY: query string, header, or media type
            configuration.MapVersionedODataRoute("odata", "odata", models, ConfigureContainer);

            // WHEN VERSIONING BY: url segment
            // configuration.MapVersionedODataRoutes( "odata-bypath", "api/v{apiVersion}", models, ConfigureContainer );

            // add the versioned IApiExplorer and capture the strongly-typed implementation (e.g. ODataApiExplorer vs IApiExplorer)
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            var apiExplorer = configuration.AddODataApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;

                    //// configure query options (which cannot otherwise be configured by OData conventions)
                    //options.QueryOptions.Controller<V2.PeopleController>()
                    //                    .Action( c => c.Get( default ) )
                    //                        .Allow( Skip | Count )
                    //                        .AllowTop( 100 )
                    //                        .AllowOrderBy( "firstName", "lastName" );

                    //options.QueryOptions.Controller<V3.PeopleController>()
                    //                    .Action( c => c.Get( default ) )
                    //                        .Allow( Skip | Count )
                    //                        .AllowTop( 100 )
                    //                        .AllowOrderBy( "firstName", "lastName" );
                });

            configuration.EnableSwagger(
                "{apiVersion}/swagger",
                swagger =>
                {
                    // build a swagger document and endpoint for each discovered API version
                    swagger.MultipleApiVersions(
                        (apiDescription, version) => apiDescription.GetGroupName() == version,
                        info =>
                        {
                            foreach (var group in apiExplorer.ApiDescriptions)
                            {
                                var description = "A sample application with Swagger, Swashbuckle, OData, and API versioning.";

                                if (group.IsDeprecated)
                                {
                                    description += " This API version has been deprecated.";
                                }

                                info.Version(group.Name, $"Sample API {group.ApiVersion}")
                                    .Contact(c => c.Name("Bill Mei").Email("bill.mei@somewhere.com"))
                                    .Description(description)
                                    .License(l => l.Name("MIT").Url("https://opensource.org/licenses/MIT"))
                                    .TermsOfService("Shareware");
                            }
                        });

                    // add a custom operation filter which documents the implicit API version parameter
                    swagger.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    swagger.IncludeXmlComments(XmlCommentsFilePath);
                })
                .EnableSwaggerUi(swagger => swagger.EnableDiscoveryUrlSelector());

            configuration.Services.Replace(typeof(IHttpControllerSelector), new ThruApiVersionControllerSelector(configuration, opts));

            builder.UseWebApi(configuration);
        }

        /// <summary>
        /// Get the root content path.
        /// </summary>
        /// <value>The root content path of the application.</value>
        public static string ContentRootPath
        {
            get
            {
                var app = AppDomain.CurrentDomain;

                if (string.IsNullOrEmpty(app.RelativeSearchPath))
                {
                    return app.BaseDirectory;
                }

                return app.RelativeSearchPath;
            }
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(ContentRootPath, fileName);
            }
        }

        static void ConfigureContainer(IContainerBuilder builder)
        {
            builder.AddService<IODataPathHandler>(Singleton, sp => new DefaultODataPathHandler() { UrlKeyDelimiter = Parentheses });
            builder.AddService<ODataUriResolver>(Singleton, sp => new UnqualifiedCallAndEnumPrefixFreeResolver() { EnableCaseInsensitive = true });
        }
    }
}