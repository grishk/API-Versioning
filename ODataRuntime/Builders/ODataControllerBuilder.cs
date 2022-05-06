using ODataRuntime.Controllers;
using ODataRuntime.Models;

namespace ODataRuntime.Builders
{
    public class ODataControllerBuilder<TKey,TEntity>: ControllerBuilder
        where TEntity : BaseEntity<TKey>
    {

        public ODataControllerBuilder(AssemblyBuilder assemblyBuilder, string controllerName) 
            : base(assemblyBuilder, controllerName, typeof(BaseEntityODataController<TKey, TEntity>))
        {
            
        }
    }

    public class ODataControllerBuilderInt<TEntity> : ODataControllerBuilder<int, TEntity>
        where TEntity : BaseEntity<int>
    {

        public ODataControllerBuilderInt(AssemblyBuilder assemblyBuilder, string controllerName)
            : base(assemblyBuilder, controllerName)
        {

        }
    }
}
