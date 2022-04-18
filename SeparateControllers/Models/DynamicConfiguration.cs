using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using SeparateControllers.Extra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.Http.ModelBinding;

namespace SeparateControllers.Models
{
    public class DynamicConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix)
        {
            builder.EntitySet<LocalMarket>("LocalMarket").EntityType.HasKey(o => o.Id);
            builder.EntitySet<GlobalMarket>("GlobalMarket").EntityType.HasKey(o => o.Id);
        }

        //    /// <summary>
        //    /// Creates the .NET controller corresponding to unbound methods
        //    /// </summary>

        //    private static ConstructorInfo GetOdataRouteConstructor()
        //    {
        //        ConstructorInfo odataRouteConstructor = typeof(ODataRouteAttribute).GetConstructor(new[] { typeof(string) });
        //        if (odataRouteConstructor == null)
        //        {
        //            throw new ArgumentNullException(nameof(odataRouteConstructor));
        //        }

        //        return odataRouteConstructor;
        //    }

        //    private static ConstructorInfo GetVersionConstructor()
        //    {
        //        ConstructorInfo versionConstructor = typeof(ApiVersionAttribute).GetConstructor(new[] { typeof(string) });
        //        if (versionConstructor == null)
        //        {
        //            throw new ArgumentNullException(nameof(versionConstructor));
        //        }

        //        return versionConstructor;
        //    }

        //    private static PropertyInfo GetNameProperty()
        //    {
        //        PropertyInfo property = typeof(ModelBinderAttribute).GetProperty(nameof(ModelBinderAttribute.Name), BindingFlags.Public | BindingFlags.Instance);
        //        if (property == null)
        //        {
        //            throw new ArgumentException("Entity type does not have a name property");
        //        }

        //        return property;
        //    }

        //    private Type CreateLocalMarket() 
        //    {
        //        PropertyInfo nameProperty = GetNameProperty();
        //        Type baseType = typeof(BaseODataController<LocalMarket>);

        //        TypeBuilder typeBuilder = _ModuleBuilder.DefineType($"{_ModuleName}.ODataMarketController", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);

        //        ConstructorInfo odataRouteConstructor = GetOdataRouteConstructor();
        //        typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(odataRouteConstructor, new object[] { "market" }));
        //        var versionConstructor = GetVersionConstructor();
        //        typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(versionConstructor, new object[] { "2.0" }));
        //        typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(versionConstructor, new object[] { "3.0" }));

        //        typeBuilder.SetParent(baseType);

        //        var createdType = typeBuilder.CreateType();
        //        return createdType;
        //    }

        //    private Type CreateMarket()
        //    {
        //        PropertyInfo nameProperty = GetNameProperty();
        //        Type baseType = typeof(BaseController<LocalMarket>);

        //        TypeBuilder typeBuilder = _ModuleBuilder.DefineType($"{_ModuleName}.MarketPureController", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);

        //        typeBuilder.SetParent(baseType);

        //        var createdType = typeBuilder.CreateType();
        //        return createdType;
        //    }
    }
}