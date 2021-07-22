using System.Web.Http;
using Microsoft.AspNet.OData.Builder;
using Microsoft.Web.Http.Versioning;
using Microsoft.Web.Http.Versioning.Conventions;
using SeparateControllers.Models;

namespace FullDotNet
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //ControllerBuilder.Current.DefaultNamespaces.Add("FullDotNet.Extra.Controllers");
            //var constraintResolver = new DefaultInlineConstraintResolver()
            //{
            //    ConstraintMap =
            //    {
            //        ["apiVersion"] = typeof( ApiVersionRouteConstraint )
            //    }
            //};

            //// Web API routes
            //config.MapHttpAttributeRoutes(constraintResolver);

            // Web API configuration and services
            config.AddApiVersioning(
                cfg =>
                {
                    cfg.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new UrlSegmentApiVersionReader());
                    cfg.Conventions.Add(new VersionByNamespaceConvention());
                    cfg.ReportApiVersions = true;
                }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            var modelBuilder = new VersionedODataModelBuilder(config)
            {
                ModelConfigurations =
                {
                    new DataModelConfiguration()
                }
            };


            var models = modelBuilder.GetEdmModels();
            config.MapVersionedODataRoute("odata-bypath", "odata/v{apiVersion}", models);

            //config.EnableSwaggerUI()
            //config.Routes.MapHttpRoute(
            //    name: "versionedApi",
            //    routeTemplate: "api/v{version:apiVersion}/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "versionedApi",
            //    routeTemplate: "apiv/v{version:apiVersion}/{controller}/{id}",

            //    defaults: new { id = RouteParameter.Optional },
            //    constraints: new { apiVersion = new ApiVersionRouteConstraint() }
            //);
        }
    }
}
