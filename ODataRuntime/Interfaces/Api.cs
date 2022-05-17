using System;
using Microsoft.AspNet.OData;
using ODataRuntime.Builders;
using ODataRuntime.Controllers;
using ODataRuntime.Models;

namespace ODataRuntime.Interfaces {
    public abstract class Api {
        private readonly Type _ControllerBaseType;
        private readonly string _ControllerName;

        protected ControllerBuilder ControllerBuilder { get; private set; }

        protected Api(string controllerName, Type baseType) {
            _ControllerName = controllerName;
            _ControllerBaseType = baseType;
        }

        public void Create(AssemblyBuilder assemblyBuilder) {
            using (ControllerBuilder = new ControllerBuilder(assemblyBuilder, _ControllerName, _ControllerBaseType)) {
                Register(ControllerBuilder);
                ControllerBuilder = null;
            }
        }

        public abstract void Register(ControllerBuilder controllerBuilder);
    }

    public abstract class BoundApi<TKey, TEntity> : Api
        where TEntity: BaseEntity<TKey> {
        protected BoundApi()
            : base(typeof(TEntity).Name, typeof(BaseEntityODataController<TKey, TEntity>)) { }
    }

    public abstract class UnboundApi : Api {
        protected UnboundApi(string controllerName) : base(controllerName, typeof(ODataController)) { }
    }
}