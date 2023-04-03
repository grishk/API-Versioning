using Asp.Versioning.OData;
using ODataRuntime.Impl.ApiControllers;
using SelfHost2.Models.Filters;
using SeparateControllers.Models;
using Swashbuckle.Application;

namespace SelfHost2
{
	using global::Owin;
	using Microsoft.AspNet.OData;
	using Microsoft.AspNet.OData.Extensions;
	using Microsoft.AspNet.OData.Routing;
	using Microsoft.OData;
	using Microsoft.OData.UriParser;
	using Newtonsoft.Json.Serialization;
	using SelfHost2.Models;
	using SeparateControllers.DynamicControllers;
	using System;
	using System.IO;
	using System.Reflection;
	using System.Web.Http;
	using System.Web.Http.Description;
	using System.Web.Http.Dispatcher;
	using System.Web.Http.ExceptionHandling;
	using static Microsoft.OData.ODataUrlKeyDelimiter;
	using static Microsoft.OData.ServiceLifetime;

	/// <summary>
	/// Represents the startup process for the application.
	/// </summary>
	public class Startup {
        public void Configuration(IAppBuilder builder) {
            var configuration = new HttpConfiguration();

            configuration.Routes.MapHttpRoute(
                           name: "DefaultApi",
                           routeTemplate: "api/{controller}/{action}/{id}",
                           defaults: new { id = RouteParameter.Optional }
                       );

            // create dynamic controllers
            MarketControllerBuilder.Build();
            PingControllerBuilder.Build();

            // handling error
            configuration.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            // include dynamic assemblies
            configuration.Services.Replace(typeof(IHttpControllerTypeResolver), new CustomHttpControllerTypeResolver());

            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            configuration.AddApiVersioning(options => {
                options.ReportApiVersions = true;
            });

            // note: this is required to make the default swagger json settings match the odata conventions applied by EnableLowerCamelCase()
            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var modelBuilder = new VersionedODataModelBuilder(configuration) {
                ModelConfigurations =
                {
					new AllConfigurations()
                }
            };
            var models = modelBuilder.GetEdmModels();

            configuration.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            configuration.MapVersionedODataRoute("odata", "odata", models, ConfigureContainer);

            var apiExplorer = configuration.AddODataApiExplorer(
                options => {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            configuration.EnableSwagger(
                "{apiVersion}/swagger",
                swagger => {
                    // build a swagger document and endpoint for each discovered API version
                    swagger.MultipleApiVersions(
                        (apiDescription, version) => apiDescription.GetGroupName() == version,
                        info => {
                            foreach (var group in apiExplorer.ApiDescriptions) {
                                var description = "A sample application with Swagger, Swashbuckle, OData, and API versioning.";

                                if (group.IsDeprecated) {
                                    description += " This API version has been deprecated.";
                                }

                                info.Version(group.Name, $"Sample API {group.ApiVersion}")
                                    .Contact(c => c.Name("corp").Email("administrators@corp.com"))
                                    .Description(description)
                                    .License(l => l.Name("MIT").Url("https://opensource.org/licenses/MIT"))
                                    .TermsOfService("Shareware");
                            }
                        });

                    // add query parameter
                    swagger.OperationFilter<QueryOperationFiler>();
                    // add a custom operation filter which documents the implicit API version parameter
                    swagger.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    swagger.IncludeXmlComments(XmlCommentsFilePath);
                })
                .EnableSwaggerUi(swagger => swagger.EnableDiscoveryUrlSelector());

            builder.UseAutofac(configuration);
            builder.UseWebApi(configuration);
        }

        /// <summary>
        /// Get the root content path.
        /// </summary>
        /// <value>The root content path of the application.</value>
        public static string ContentRootPath {
            get {
                var app = AppDomain.CurrentDomain;

                if (string.IsNullOrEmpty(app.RelativeSearchPath)) {
                    return app.BaseDirectory;
                }

                return app.RelativeSearchPath;
            }
        }

        static string XmlCommentsFilePath {
            get {
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(ContentRootPath, fileName);
            }
        }

        static void ConfigureContainer(IContainerBuilder builder) {
            builder.AddService<IODataPathHandler>(Singleton, sp => new DefaultODataPathHandler() { UrlKeyDelimiter = Parentheses });
            builder.AddService<ODataUriResolver>(Singleton, sp => new UnqualifiedCallAndEnumPrefixFreeResolver() { EnableCaseInsensitive = true });
        }
    }
}