using Microsoft.AspNet.OData;
using ODataRuntime.Builders;
using ODataRuntime.Controllers;
using ODataRuntime.Models;
using System;

namespace ODataRuntime.Interfaces
{
    public abstract class Api
    {
        private readonly string _ControllerName;
        private readonly Type _BaseType;

        public Api(string controllerName): this(controllerName, typeof(ODataController))
        {
        }

        public Api(string controllerName, Type baseType)
        {
            _ControllerName = controllerName;
            _BaseType = baseType;
        }

        public void Create(AssemblyBuilder assemblyBuilder)
        {
            using (var controllerBuilder = new ControllerBuilder(assemblyBuilder, _ControllerName, _BaseType))
            {
                Register(controllerBuilder);
            }
        } 

        public abstract void Register(ControllerBuilder controllerBuilder);
    }

    public abstract class Api<TEntity> : Api
        where TEntity : EntityKeyInt
    {
        public Api(): base(typeof(TEntity).Name, typeof(BaseEntityODataControllerInt<TEntity>))
        {
        }
    }

    public abstract class Api<TKey, TEntity> : Api
        where TEntity : BaseEntity<TKey>
    {
        public Api() : base(typeof(TEntity).Name, typeof(BaseEntityODataController<TKey,TEntity>))
        {
        }
    }
}
