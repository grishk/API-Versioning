using ODataRuntime.Builders;
using ODataRuntime.Controllers;
using ODataRuntime.Models;

namespace ODataRuntime.Interfaces {
    public abstract class ModelApi<TModel> : Api
        where TModel : EntityKeyInt
    {
        protected ModelApi(AssemblyBuilder assemblyBuilder) : base(typeof(TModel).Name, typeof(BaseEntityODataControllerInt<TModel>), assemblyBuilder)
        {
        }

        protected void SetDefaultRoute()
        {
            
        }
    }
}