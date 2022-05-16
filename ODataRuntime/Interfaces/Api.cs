using Microsoft.AspNet.OData;
using ODataRuntime.Builders;
using ODataRuntime.Controllers;
using ODataRuntime.Models;
using System;

namespace ODataRuntime.Interfaces
{
    public abstract class Api : IDisposable
    {
        private readonly ControllerBuilder _ControllerBuilder;

        protected ControllerBuilder ControllerBuilder => _ControllerBuilder;

        protected Api(string controllerName, AssemblyBuilder assemblyBuilder) : this(controllerName, typeof(ODataController), assemblyBuilder) {
            
        }

        protected Api(string controllerName, Type baseType, AssemblyBuilder assemblyBuilder)
        {
            _ControllerBuilder = new ControllerBuilder(assemblyBuilder, controllerName, baseType);
        }

        public void Create() {
            Register(_ControllerBuilder);
        } 

        public abstract void Register(ControllerBuilder controllerBuilder);

        public void Dispose() {
            _ControllerBuilder?.Dispose();
        }
    }

    public abstract class ModelApi<TKey, TEntity> : Api
        where TEntity : BaseEntity<TKey>
    {
        protected ModelApi(AssemblyBuilder assemblyBuilder) : base(typeof(TEntity).Name, typeof(BaseEntityODataController<TKey,TEntity>), assemblyBuilder)
        {
        }
    }
}
