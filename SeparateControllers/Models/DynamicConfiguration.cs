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
            //builder.EntitySet<GlobalMarket>("GlobalMarket").EntityType.HasKey(o => o.Id);
        }


        //private readonly ModuleBuilder _ModuleBuilder;
        //private readonly string _ModuleName;
        //private int _TypeCounter;

        //private bool IsApply;

        //public DynamicConfiguration()
        //{
        //    _ModuleName = GetType().Assembly.GetName().Name + ".EControllers";
        //    AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(_ModuleName), AssemblyBuilderAccess.Run);
        //    _ModuleBuilder = assemblyBuilder.DefineDynamicModule(_ModuleName + ".dll", true);
        //}

        //public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix)
        //{
        //    builder.EntitySet<LocalMarket>("LocalMarket").EntityType.HasKey(o => o.Id);
        //    if (IsApply) return;
        //    IsApply = true;
        //    var epBuilder = new HealthCheckEndPointBuilder(builder as ODataConventionModelBuilder);
        //    var unbounds = epBuilder._Unbounds.Values.ToArray();
        //    Type unboundsControllerType = CreateUnbounds(unbounds, "health", string.Empty, string.Empty);
        //    CreateLocalMarket(builder as ODataConventionModelBuilder);
        //}

        ///// <summary>
        ///// Creates the .NET controller corresponding to unbound methods
        ///// </summary>
        //private Type CreateUnbounds(IList<EntityMethodInfo> methods, string route, string pathToServiceInstance, string apiPrefix)
        //{
        //    PropertyInfo nameProperty = GetNameProperty();
        //    Type baseType = typeof(UnboundOperationsBaseController);

        //    TypeBuilder typeBuilder = _ModuleBuilder.DefineType($"{_ModuleName}.Unbounds_{_TypeCounter++}_Controller", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);

        //    ConstructorInfo odataRouteConstructor = GetOdataRouteConstructor();
        //    typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(odataRouteConstructor, new object[] { route }));

        //    typeBuilder.SetParent(baseType);

        //    int methodIndex = 0;
        //    foreach (EntityMethodInfo method in methods)
        //    {
        //        var odataMethodBuilder = ODataMethodBuilderBase.CreateODataMethodBuilder(new PropertyInfo[0], method);
        //        odataMethodBuilder.BuildMethod(baseType, typeBuilder, methodIndex++, nameProperty, null);
        //    }

        //    var createdType = typeBuilder.CreateType();
        //    var property = createdType.BaseType.GetRuntimeProperty(nameof(EntityTypeBaseController<object, object>.PathToServiceInstance));
        //    if (property != null)
        //    {
        //        property.SetValue(createdType.BaseType, pathToServiceInstance);
        //    }
        //    var propertyPrefix = createdType.BaseType.GetRuntimeProperty(nameof(EntityTypeBaseController<object, object>.ApiPrefix));
        //    if (propertyPrefix != null)
        //    {
        //        propertyPrefix.SetValue(createdType.BaseType, apiPrefix);
        //    }

        //    return createdType;
        //}

        //private static ConstructorInfo GetOdataRouteConstructor()
        //{
        //    ConstructorInfo odataRouteConstructor = typeof(ODataRouteAttribute).GetConstructor(new[] { typeof(string) });
        //    if (odataRouteConstructor == null)
        //    {
        //        throw new ArgumentNullException(nameof(odataRouteConstructor));
        //    }

        //    return odataRouteConstructor;
        //}

        //private static ConstructorInfo GetVersionConstructor()
        //{
        //    ConstructorInfo versionConstructor = typeof(ApiVersionAttribute).GetConstructor(new[] { typeof(string) });
        //    if (versionConstructor == null)
        //    {
        //        throw new ArgumentNullException(nameof(versionConstructor));
        //    }

        //    return versionConstructor;
        //}

        //private static PropertyInfo GetNameProperty()
        //{
        //    PropertyInfo property = typeof(ModelBinderAttribute).GetProperty(nameof(ModelBinderAttribute.Name), BindingFlags.Public | BindingFlags.Instance);
        //    if (property == null)
        //    {
        //        throw new ArgumentException("Entity type does not have a name property");
        //    }

        //    return property;
        //}

        //private Type CreateLocalMarket(ODataConventionModelBuilder builder) 
        //{
        //    PropertyInfo nameProperty = GetNameProperty();
        //    Type baseType = typeof(BaseController<LocalMarket>);

        //    TypeBuilder typeBuilder = _ModuleBuilder.DefineType($"{_ModuleName}.MarketController", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class);

        //    ConstructorInfo odataRouteConstructor = GetOdataRouteConstructor();
        //    typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(odataRouteConstructor, new object[] { "market" }));
        //    var versionConstructor = GetVersionConstructor();
        //    typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(versionConstructor, new object[] { "2.0" }));
        //    typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(versionConstructor, new object[] { "3.0" }));

        //    typeBuilder.SetParent(baseType);

        //    var createdType = typeBuilder.CreateType();
        //    return createdType;
        //}
    }
}